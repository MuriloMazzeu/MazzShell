using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MazzShell
{
    public sealed class MazzShell
    {
        public static MazzShell CreateShellHost(string[] args)
        {
            var builder = Host
                .CreateDefaultBuilder(args)
                .UseWindowsService();

            return new MazzShell(builder);
        }

        private MazzShell(IHostBuilder builder)
        {
            HostBuilder = builder;
            Singletons = new Dictionary<Type, Type>();
            Scopeds = new Dictionary<Type, Type>();
            Transients = new Dictionary<Type, Type>();
            Workers = new List<Action<IServiceCollection>>();
            ServiceConfigures = new List<Action<IServiceCollection>>();
        }

        private List<Action<IServiceCollection>> ServiceConfigures { get; }
        public MazzShell ConfigureServices(Action<IServiceCollection> action)
        {
            ServiceConfigures.Add(action);
            return this;
        }

        private Dictionary<Type, Type> Singletons { get; }
        public MazzShell AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            Singletons.Add(typeof(TService), typeof(TImplementation));
            return this;
        }

        private Dictionary<Type, Type> Scopeds { get; }
        public MazzShell AddScoped<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            Scopeds.Add(typeof(TService), typeof(TImplementation));
            return this;
        }

        private Dictionary<Type, Type> Transients { get; }
        public MazzShell AddTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            Transients.Add(typeof(TService), typeof(TImplementation));
            return this;
        }

        private List<Action<IServiceCollection>> Workers { get; }
        public MazzShell AddWorker<TService>()
            where TService : class, IBackgroundService
        {
            Workers.Add(services =>
            {
                services.AddSingleton<TService>();
                services.AddHostedService<Worker<TService>>();
            });

            return this;
        }

        public IHostBuilder HostBuilder { get; }
        public async Task RunAsync()
        {
            using (var host = BuildHost())
            {
                await host.RunAsync();
            }
        }

        public void Run()
        {
            using (var host = BuildHost())
            {
                host.RunAsync().ConfigureAwait(true).GetAwaiter().GetResult();
            }
        }

        private IHost BuildHost()
        {
            return HostBuilder.ConfigureServices((hostContext, services) =>
            {
                foreach (var singleton in Singletons)
                {
                    services.AddSingleton(singleton.Key, singleton.Value);
                }

                foreach (var scoped in Scopeds)
                {
                    services.AddScoped(scoped.Key, scoped.Value);
                }

                foreach (var transient in Transients)
                {
                    services.AddTransient(transient.Key, transient.Value);
                }

                foreach (var worker in Workers)
                {
                    worker(services);
                }

                foreach (var action in ServiceConfigures)
                {
                    action(services);
                }
            })
            .Build();
        }
    }
}
