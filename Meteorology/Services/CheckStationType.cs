using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weather.Data.UnitOfWork;
using Weather.Models;

namespace Weather.Services
{
    public class CheckStationType
    {
        private readonly GenericUoW _uow;
        public CheckStationType(GenericUoW uow)
        {
            _uow = uow;
        }
        public async Task<List<SensorSetting>> GetSensorsType1Async()
        {
            return await _uow.Repository<SensorSetting>()
                .GetAllQueryable(s => s.SensorEnable && s.SensorTypeId == 2 && s.Station.DataLoggerId == 7)
                .Include(s => s.Station)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<SensorSetting>> GetSensorsType2Async()
        {
            return await _uow.Repository<SensorSetting>()
                .GetAllQueryable(s => s.SensorEnable && s.SensorTypeId == 4 &&
                                      (s.Station.DataLoggerId == 8 || s.Station.DataLoggerId == 6))
                .Include(s => s.Station)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<SensorSetting>> GetSensorsType3Async()
        {
            return await _uow.Repository<SensorSetting>()
                .GetAllQueryable(s => s.SensorEnable && s.SensorTypeId == 2 &&
                                      (s.Station.DataLoggerId == 3 ||
                                       s.Station.DataLoggerId == 4 ||
                                       s.Station.DataLoggerId == 5))
                .Include(s => s.Station)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<SensorSetting>> GetSensorsType4Async()
        {
            return await _uow.Repository<SensorSetting>()
                .GetAllQueryable(s => s.SensorEnable && s.SensorTypeId == 2 && s.Station.DataLoggerId == 2)
                .Include(s => s.Station)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<SensorSetting>> GetSensorsTypeDefaultAsync()
        {
            return await _uow.Repository<SensorSetting>()
                .GetAllQueryable(s => s.SensorEnable && (s.SensorTypeId == 2 || s.SensorTypeId == 4))
                .Include(s => s.Station)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
