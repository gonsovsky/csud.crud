using System;
using System.ComponentModel;
using Csud.Crud.DbTool.Generation;
using Csud.Crud.DbTool.Import;
using Csud.Crud.DbTool.PromtEx;
using Microsoft.Extensions.DependencyInjection;

namespace Csud.Crud.DbTool
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            Tool.Init();
            Tool.RegisterServices();

            switch (Command)
            {
                case "generate":
                    Generate();
                    break;
                case "import":
                    Import();
                    break;
                case "":
                    Tool.Promt.Run();
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }

            Console.WriteLine("Enter to exit");
            Console.ReadKey();
        }

        internal static void Import()
        {
            var importer = Tool.ServiceProvider.GetRequiredService<IImportService>();
            importer.Run(Argument);
        }

        internal static void Generate()
        {
            var generator = Tool.ServiceProvider.GetRequiredService<IGeneratorService>();
            generator.Run(Promt.Result);
        }

        internal static string Command
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
        internal static string Argument
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
