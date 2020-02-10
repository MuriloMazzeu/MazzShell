# MazzShell
.NET Core wrapper for building background services

## How to use
- Create a shell that encapsulates the generic .NET Core host, use the static *MazzShell* class to create your app
- Setup the D.I. container, you can use Adds shortcuts like *AddTransient* or the *ConfigureServices* method
- Provide at least one class that inherits from IBackgroundService for the *AddWorker* method
- Run your code

## Show me the code
In your Program class:
    
    public static void Main(string[] args)
    {
        MazzShell.CreateShellHost(args)
            .AddTransient<IConsoleWritter, ConsoleWritter>()
            .AddWorker<MyService>()
            .Run();
    }
    
The writter class:

    public interface IConsoleWritter
    {
        void SayHello(string to);
    }

    public class ConsoleWritter : IConsoleWritter
    {
        public void SayHello(string to) => Console.WriteLine("Hello {0}", to);
    }
    
The worker class:

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
    

## OS support
*currently only on Windows*
