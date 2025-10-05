using System.Linq;
using System.Threading.Tasks;
using Weather.Data.Interface;
using Weather.Data.Enums;
using Weather.Models;

namespace Weather.Data.Handlers
{
    public class ProcessedDataHandler
    {
        private readonly IRepository<ProcessedData> _repo;
        private readonly IGenericUoW _unitOfWork;

        public ProcessedDataHandler(IRepository<ProcessedData> repo, IGenericUoW unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }
        public async Task<double> UpdateProcessedDataAsync(SensorDateTime data, long userId = 1)
        {
            // بررسی وجود رکورد قبلی
            var existingList = await _repo.GetAllAsync(p => p.SensorSettingId == data.SensorSettingId);
            var processed = existingList.FirstOrDefault();

            if (processed != null)
            {
                double oldValue = processed.DataValue ?? 0;

                processed.DataValue = data.Data;
                processed.LastProcessedId = data.Id;

                await _repo.UpdateAsync(processed);
                await _unitOfWork.SaveAsync(userId, EnuAction.Update, nameof(ProcessedData));

                return oldValue;
            }

            else
            {
                var newProcessed = new ProcessedData
                {
                    SensorSettingId = data.SensorSettingId,
                    LastProcessedId = data.Id,
                    DataValue = data.Data
                };

                await _repo.InsertAsync(newProcessed);
                await _unitOfWork.SaveAsync(userId, EnuAction.Create, nameof(ProcessedData));

                return 0;
            }
        }
    }
}
