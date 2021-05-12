using System;
using System.Linq;
using Csud.Crud.DBTool.Promt.ConsoleEx;
using Microsoft.Extensions.Configuration;

namespace Csud.Crud.DBTool
{
    public static class Program
    {
        private static ICsud _csud;

        public static Config Cfg;

        private static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true); 
            var cfg = builder.Build();
            Cfg = new Config(cfg);

            var promt = new Promt.Promt();
            if (args.Contains("auto"))
                BeginGeneration();
            else
                promt.Run();

            Console.WriteLine("Enter to exit");
            Console.ReadKey();
        }

        public static void BeginGeneration()
        {
            CsudService.Drop(Cfg);
            CsudService.StartUp(Cfg);
            _csud = CsudService.Csud;
            var gen = new DataGenerator(_csud);
            gen.Generate(X.Stat);
        }

    }
}
