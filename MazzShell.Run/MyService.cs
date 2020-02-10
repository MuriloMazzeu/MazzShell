using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MazzShell.Run
{
    public class MyService : IBackgroundService
    {
        public MyService(ILogger<MyService> logger, IConsoleWritter writter)
        {
            Writter = writter;
            Logger = logger;
        }

        private IConsoleWritter Writter { get; }
        private ILogger<MyService> Logger { get; }

        public TimeSpan ExecutionInterval => TimeSpan.FromSeconds(1);

        public Task OnExceptionAsync(Exception exception)
        {
            Logger.LogError(exception.Message);
            return Task.CompletedTask;
        }

        public Task RunAsync()
        {
            Writter.SayHello("World");
            return Task.CompletedTask;
        }
    }
}
