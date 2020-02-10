namespace MazzShell.Run
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MazzShell.CreateShellHost(args)
                .AddTransient<IConsoleWritter, ConsoleWritter>()
                .AddWorker<MyService>()
                .Run();
        }
    }
}
