using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Weather.Data.Enums;
using Weather.Data.UnitOfWork;

namespace Weather.Services
{
    public class CheckSensorDataBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CheckSensorDataBackgroundService> _logger;
        private const long SystemUserId = 1;

        public CheckSensorDataBackgroundService(IServiceProvider serviceProvider, ILogger<CheckSensorDataBackgroundService> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();

                    var processor = scope.ServiceProvider.GetRequiredService<SensorProcessor>();
                    var sensors = await processor.GetSensorsAsync();

                    foreach (var sensor in sensors)
                    {
                        if (stoppingToken.IsCancellationRequested) break;
                        await processor.ProcessSensorAsync(sensor);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ خطا در سرویس پس‌زمینه CheckSensorDataBackgroundService");
                }

                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }
}
