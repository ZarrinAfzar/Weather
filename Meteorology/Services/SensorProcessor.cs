using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data.Enums;
using Weather.Data.Handlers;
using Weather.Data.UnitOfWork;
using Weather.Models;

namespace Weather.Services
{
    public class SensorProcessor
    {
        private readonly GenericUoW _uow;
        private readonly RainfallEventHandler _rainfallHandler;
        private readonly ManageSMS _manageSMS;

        public SensorProcessor(GenericUoW uow, RainfallEventHandler rainfallHandler, ManageSMS managesms)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _rainfallHandler = rainfallHandler ?? throw new ArgumentNullException(nameof(rainfallHandler));
            _manageSMS = managesms;
        }

        public async Task<List<SensorSetting>> GetSensorsAsync()
        {
            return await _uow.Repository<SensorSetting>()
                .GetAllQueryable(s => s.SensorEnable && (s.SensorTypeId == 2 || s.SensorTypeId == 4))
                .Include(s => s.Station)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task ProcessSensorAsync(SensorSetting sensor)
        {
            var lastProcessed = await GetLastProcessedAsync(sensor.Id);
            var newDataList = await GetNewSensorDataAsync(sensor.Id, lastProcessed?.LastProcessedId ?? 0);

            if (!newDataList.Any()) return;

            double lastValue = lastProcessed?.DataValue ?? 0;

            foreach (var data in newDataList)
            {
                await ProcessRainfallAsync(data, lastValue);
                lastValue = data.Data;
                lastProcessed.LastProcessedId = data.Id;
                lastProcessed.DataValue = data.Data;

                await UpsertProcessedDataAsync(lastProcessed);
            }
            var IsRaining = _uow.Repository<RainfallEvent>().GetAllQueryable(x => x.IsRaining && !x.IsStartSMSSent);
            foreach (var item in IsRaining)
            {
                await _manageSMS.SendStartSMS(item.StationId, item.RainStart);
                item.IsStartSMSSent = true;
            }
            await _uow.SaveAsync(1, EnuAction.Update, nameof(ProcessedData));
        }

        private async Task<ProcessedData> GetLastProcessedAsync(long sensorId)
        {
            return await _uow.Repository<ProcessedData>()
                .GetAllQueryable(p => p.SensorSettingId == sensorId)
                .FirstOrDefaultAsync();
        }

        private async Task<List<SensorDateTime>> GetNewSensorDataAsync(long sensorId, long lastProcessedId)
        {
            return await _uow.Repository<SensorDateTime>()
                .GetAllQueryable(d => d.SensorSettingId == sensorId && d.Id > lastProcessedId)
                .Include(d => d.SensorSetting.Station)
                .OrderBy(d => d.Id)
                .AsNoTracking()
                .ToListAsync();
        }

        private async Task ProcessRainfallAsync(SensorDateTime data, double oldValue)
        {
            await _rainfallHandler.ProcessRainfallAsync(data, oldValue);
        }

        private async Task UpsertProcessedDataAsync(ProcessedData entity)
        {
            var repo = _uow.Repository<ProcessedData>();
            var existing = await repo
                .GetAllQueryable(p => p.SensorSettingId == entity.SensorSettingId)
                .FirstOrDefaultAsync();

            if (existing == null)
                repo.Insert(entity);
            else
            {
                existing.LastProcessedId = entity.LastProcessedId;
                existing.DataValue = entity.DataValue;
                repo.Update(existing);
            }
        }
    }
}
