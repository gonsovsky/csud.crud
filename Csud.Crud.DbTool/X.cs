using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Csud.Crud.DbTool.Generation;
using Csud.Crud.DbTool.Import;
using Csud.Crud.DbTool.PromtEx;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Csud.Crud.DbTool
{
    public static class X
    {
        public static Promt Promt;

        public static IServiceProvider ServiceProvider;

        public static Config Cfg;

        public static IConfiguration cfg;

        public static void Init()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);
            cfg = builder.Build();
            Cfg = new Config(cfg);
            Cfg.DropOnStart = true;
            Promt = new Promt();
        }

        public static void RegisterServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton(typeof(IConfiguration), cfg);

            services.AddSingleton(typeof(Config), Cfg);

            Csud.Crud.CsudService.ConfigureServices(services);

            services.AddSingleton<IGeneratorService, GeneratorService>();

            services.AddSingleton<IImportService, ImportService>();

            ServiceProvider = services.BuildServiceProvider(true);
        }
    }
}
