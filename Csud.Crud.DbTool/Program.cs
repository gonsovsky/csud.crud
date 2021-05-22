using System;
using System.Configuration;
using System.Linq;
using Csud.Crud.DbTool.PromtEx;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Models.Maintenance;
using Csud.Crud.Models.Rules;
using Csud.Crud.RestApi;
using Csud.Crud.Services;
using Csud.Crud.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Csud.Crud.DbTool
{
    public static class Program
    {
        private static IServiceProvider _serviceProvider;

        public static Config Cfg;

        public static IConfiguration cfg;

        public static IServiceScope scope;

        private static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true); 
            cfg = builder.Build();
            Cfg = new Config(cfg);

            var promt = new Promt();
            if (args.Contains("auto"))
                BeginGeneration();
            else
                promt.Run();

            Console.WriteLine("Enter to exit");
            Console.ReadKey();
        }

        public static void BeginGeneration()
        {
            RegisterServices();
            scope = _serviceProvider.CreateScope();
            var maintaner = scope.ServiceProvider.GetRequiredService<IMaintenanceService>();
            var generator = scope.ServiceProvider.GetRequiredService<IGeneratorService>();
            maintaner.Drop();
            generator.Run(Promt.Result);
            DisposeServices();
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton(typeof(IConfiguration), cfg);

            Startup.ConfigureCsudServices(services);

            services.AddSingleton<IGeneratorService, GeneratorService>();

            _serviceProvider = services.BuildServiceProvider(true);
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}
