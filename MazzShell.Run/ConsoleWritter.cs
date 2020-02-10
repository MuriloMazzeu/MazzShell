using System;

namespace MazzShell.Run
{
    public interface IConsoleWritter
    {
        void SayHello(string to);
    }

    public class ConsoleWritter : IConsoleWritter
    {
        public void SayHello(string to) => Console.WriteLine("Hello {0}", to);
    }
}
