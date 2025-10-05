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

            var increment = Math.Max(Math.Max(data.Data - oldData, data.Data), 0);
            bool rainingNow = increment > 0;

            var lastEvent = (await _repo.GetAllQueryableAsync(r => r.SensorSettingId == data.SensorSettingId))
                .OrderByDescending(r => r.Id)
                .FirstOrDefault();

            if (rainingNow)
            {
                if (lastEvent == null || !lastEvent.IsRaining && (data.DateTime - lastEvent.RainEnd) > RainEndThreshold)
                {
                    // ایجاد رویداد جدید
                    await CreateNewRainEventAsync(data, increment, userId);
                }
                else if (!lastEvent.IsRaining && (data.DateTime - lastEvent.RainEnd) <= RainEndThreshold)
                {
                    // Resume رویداد قبلی
                    lastEvent.IsRaining = true;
                    lastEvent.LastIdWithRain = data.Id;
                    lastEvent.RainEnd = data.DateTime;
                    lastEvent.RainfallVolume += increment;
                    await _repo.UpdateAsync(lastEvent);
                    await _uow.SaveAsync(userId, EnuAction.Update, nameof(RainfallEvent));
                }
                else if (lastEvent.IsRaining && (data.DateTime - lastEvent.RainEnd) <= RainEndThreshold)

                {

                    // بروزرسانی رویداد فعال
                    lastEvent.LastIdWithRain = data.Id;
                    lastEvent.RainEnd = data.DateTime;
                    lastEvent.RainfallVolume += increment;
                    await _repo.UpdateAsync(lastEvent);
                    _uow.Save(userId, EnuAction.Update, nameof(RainfallEvent));
                }
                if (increment > 0 && lastEvent != null && (data.DateTime - lastEvent.RainEnd) >= RainEndThreshold)
                {
                    await EndRainEventAsync(lastEvent, userId);
                    await CreateNewRainEventAsync(data, increment, userId);
                }
            }
            else if (lastEvent != null && lastEvent.IsRaining)
            {
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
