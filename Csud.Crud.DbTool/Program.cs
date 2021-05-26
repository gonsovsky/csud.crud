using System;
using System.ComponentModel;
using System.Linq;
using Csud.Crud.DbTool.Generation;
using Csud.Crud.DbTool.Import;
using Csud.Crud.DbTool.PromtEx;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Csud.Crud.DbTool
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            X.Init();
            X.RegisterServices();

            switch (Command)
            {
                case "generate":
                    Generate();
                    break;
                case "import":
                    Import();
                    break;
                case "":
                    X.Promt.Run();
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }

            Console.WriteLine("Enter to exit");
            Console.ReadKey();
        }

        public static void Import()
        {
            var importer = X.ServiceProvider.GetRequiredService<IImportService>();
            importer.Run(Argument);
        }

        public static void Generate()
        {
            var generator = X.ServiceProvider.GetRequiredService<IGeneratorService>();
            generator.Run(Promt.Result);
        }

        public static string Command
        {
            get
            {
                var args = Environment.GetCommandLineArgs();
                var arg = "";
                if (args.Length >= 2)
                    arg = args[1];
                return arg;
            }
        }

        public static string Argument
        {
            get
            {
                var args = Environment.GetCommandLineArgs();
                var arg = "";
                if (args.Length >= 3)
                    arg = args[2];
                if (arg == "")
                    arg = "./Resources/КПИ_ИС.xml";
                return arg;
            }
        }
    }
}
