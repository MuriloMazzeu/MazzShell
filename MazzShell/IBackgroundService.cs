using System;
using System.Threading.Tasks;

namespace MazzShell
{
    public interface IBackgroundService
    {
        TimeSpan ExecutionInterval { get; }
        Task RunAsync();
        Task OnExceptionAsync(Exception exception);
    }
}
