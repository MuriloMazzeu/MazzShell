using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MazzShell
{
    public class Worker<T> : BackgroundService where T : class, IBackgroundService
    {
        public Worker(ILogger<T> logger, T service)
        {
            Logger = logger;
            Service = service;
        }

        private ILogger Logger { get; }
        private T Service { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(Service.ExecutionInterval, stoppingToken);

                try
                {
                    await Service.RunAsync();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Worker error at: {time}", DateTimeOffset.Now);
                    await Service.OnExceptionAsync(ex);
                }
            }
            Logger.LogInformation("Worker stoped at: {time}", DateTimeOffset.Now);
        }
    }
}
