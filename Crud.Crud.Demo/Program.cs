using System;
using System.Threading;
using Csud.Crud.DBTool.Promt;
using Microsoft.Extensions.Configuration;

namespace Csud.Crud.DBTool
{
    public static class Program
    {
        private static ICsud _csud;
        public static Config Cfg;

        private static void Main()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true); 
            var cfg = builder.Build();
            Cfg = new Config(cfg);

            new DemoProgram().Run();

            Console.WriteLine("Enter to exit");
            Console.ReadKey();
        }

        public static void BeginGeneration()
        {
            Console.WriteLine("A");
            Thread.Sleep(1000);
            CsudService.StartUp(Cfg);
            _csud = CsudService.Csud;

            //var gen = new DataGenerator(_csud);
            //gen.Generate(100);
        }

    }
}
