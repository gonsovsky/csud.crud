using System;
using System.ComponentModel;
using Csud.Crud.DbTool.Generation;
using Csud.Crud.DbTool.Import;
using Csud.Crud.DbTool.PromtEx;
using Csud.Crud.DbTool.PromtEx.Pages;
using Csud.Crud.Models.Rules;
using Csud.Crud.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Csud.Crud.DbTool
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            Tool.Init();
            switch (Command)
            {
                case "generate":
                    Generate();
                    break;
                case "import":
                    Import();
                    break;
                case "create":
                    Create();
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

        internal static void Import(string p = "")
        {
            Tool.RegisterServices(false);
            var importer = Tool.ServiceProvider.GetRequiredService<IImportService>();
            importer.Run(p != "" ? p : Argument);
        }

        internal static void Generate()
        {
            Tool.RegisterServices(true);
            var generator = Tool.ServiceProvider.GetRequiredService<IGeneratorService>();
            generator.Run(Promt.Result);
        }

        internal static void Create()
        {
            Tool.RegisterServices(true);
            var db = Tool.ServiceProvider.GetRequiredService<IDbService>();
            db.Select<Account>();
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
                    arg = ImportPage.DefFile;
                return arg;
            }
        }
    }
}
