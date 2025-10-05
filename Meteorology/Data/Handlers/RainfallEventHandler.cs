using System;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data.Enums;
using Weather.Data.Interface;
using Weather.Models;
using Weather.Services;

namespace Weather.Data.Handlers
{
    public class RainfallEventHandler
    {
        private readonly IRepository<RainfallEvent> _repo;
        private readonly IGenericUoW _uow;
        private readonly ManageSMS _manageSMS;
        private static readonly TimeSpan RainEndThreshold = TimeSpan.FromHours(3);

        public RainfallEventHandler(IRepository<RainfallEvent> repo, IGenericUoW uow, ManageSMS manageSMS)
        {
            _repo = repo;
            _uow = uow;
            _manageSMS = manageSMS;
        }

        public async Task ProcessRainfallAsync(SensorDateTime data, double oldData, long userId = 1)
        {
            // محاسبه increment بر اساس نوع حسگر
            double increment = 0;
            switch (data.SensorSetting.SensorTypeId)
            {
                case 2: // Cumulative
                    increment = Math.Max(Math.Round(data.Data - oldData, 1), 0);
                    break;
                case 4: // Instantaneous (اختلاف واقعی، ممکن است منفی باشد)
                    increment = Math.Round(data.Data - oldData, 1);
                    break;
                default:
                    increment = 0;
                    break;
            }

            bool rainingNow = increment > 0;

            // بازیابی آخرین رویداد مرتبط با حسگر
            var lastEvent = (await _repo.GetAllQueryableAsync(r => r.SensorSettingId == data.SensorSettingId))
                            .OrderByDescending(r => r.Id)
                            .FirstOrDefault();

            if (rainingNow)
            {
                if (lastEvent == null)
                {
                    // هیچ رویدادی وجود ندارد → ایجاد رویداد جدید
                    await CreateNewRainEventAsync(data, increment, userId);
                }
                else
                {
                    var gap = data.DateTime - lastEvent.RainEnd;

                    if (!lastEvent.IsRaining || gap > RainEndThreshold)
                    {
                        // اگر رویداد قبلی هنوز فعال است ولی فاصله زیاد است → پایان رویداد قبلی
                        if (lastEvent.IsRaining)
                            await EndRainEventAsync(lastEvent, userId);

                        // ایجاد رویداد جدید
                        await CreateNewRainEventAsync(data, increment, userId);
                    }
                    else
                    {
                        // ادامه یا Resume رویداد موجود
                        lastEvent.IsRaining = true;
                        lastEvent.LastIdWithRain = data.Id;
                        lastEvent.RainEnd = data.DateTime;

                        // برای TypeId 2 و 4 مقدار صحیح را اضافه کن
                        if (data.SensorSetting.SensorTypeId == 2)
                            lastEvent.RainfallVolume += increment; // تجمعی
                        else if (data.SensorSetting.SensorTypeId == 4)
                            lastEvent.RainfallVolume += increment; // اختلاف واقعی

                        await _repo.UpdateAsync(lastEvent);
                        await _uow.SaveAsync(userId, EnuAction.Update, nameof(RainfallEvent));
                    }
                }
            }
            else if (lastEvent != null && lastEvent.IsRaining)
            {
                // باران متوقف شده → بررسی پایان رویداد
                if ((data.DateTime - lastEvent.RainEnd) > RainEndThreshold)
                {
                    await EndRainEventAsync(lastEvent, userId);
                }
            }
        }


        private async Task CreateNewRainEventAsync(SensorDateTime data, double increment, long userId)
        {
            var newEvent = new RainfallEvent
            {
                SensorSettingId = data.SensorSettingId,
                StationId = data.SensorSetting.StationId,
                RainStart = data.DateTime,
                RainEnd = data.DateTime,
                FirstIdWithRain = data.Id,
                LastIdWithRain = data.Id,
                IsRaining = true,
                RainfallVolume = increment,
                IsStartSMSSent = false,
                IsEndSMSSent = false
            };

            await _repo.InsertAsync(newEvent);
            _uow.Save(userId, EnuAction.Create, nameof(RainfallEvent));
        }

        private async Task EndRainEventAsync(RainfallEvent rainEvent, long userId)
        {
            rainEvent.IsRaining = false;
            if (!rainEvent.IsEndSMSSent)
            {
                rainEvent.IsEndSMSSent = await _manageSMS.SendEndSMS(rainEvent.StationId, rainEvent.RainStart, rainEvent.RainEnd);
            }

            await _repo.UpdateAsync(rainEvent);
            await _uow.SaveAsync(userId, EnuAction.Update, nameof(RainfallEvent));
        }
    }
}
