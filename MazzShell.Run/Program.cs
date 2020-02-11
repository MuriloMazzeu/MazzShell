namespace MazzShell.Run
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MazzShellHost.CreateShell(args)
                .AddTransient<IConsoleWritter, ConsoleWritter>()
                .AddWorker<MyService>()
                .Run();
        }
    }
}
