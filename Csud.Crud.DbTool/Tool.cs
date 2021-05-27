using System;
using Csud.Crud.DbTool.Generation;
using Csud.Crud.DbTool.Import;
using Csud.Crud.DbTool.Log;
using Csud.Crud.DbTool.PromtEx;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Csud.Crud.DbTool
{
    internal static class Tool
    {
        internal static Promt Promt;

        internal static IServiceProvider ServiceProvider;

        internal static Config Cfg;

        internal static IConfiguration Configuration;

        internal static void Init()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);
            Configuration = builder.Build();
            Cfg = new Config(Configuration) {DropOnStart = true};
            Promt = new Promt();
        }

        internal static void RegisterServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton(typeof(IConfiguration), Configuration);

            services.AddSingleton(typeof(Config), Cfg);

            CsudService.ConfigureServices(services);

            services.AddSingleton<ILogService, LogService>();

            services.AddSingleton<IGeneratorService, GeneratorService>();

            services.AddSingleton<IImportService, ImportService>();

            ServiceProvider = services.BuildServiceProvider(true);
        }
    }
}
