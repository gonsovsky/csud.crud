using System.Collections.Generic;
using Csud.Crud.DbTool.PromtEx.ConsoleEx;

namespace Csud.Crud.DbTool.PromtEx.Pages
{
    class MainPage : MenuPage
    {
        public static Option[] Opts()
        {
            var opts = new List<Option>();
       
            foreach (var type in Promt.Types)
            {
                var p = new BasePage(type,type.Name);
                Promt.Program.AddPage(p);
                var op = new Option(type.Name, p,() => Promt.Program.NavigateTo(p));
                opts.Add(op);
                Promt.Result.Add(type,50);
            }
            var pgGen  = new StartDataBaseGeneration(null, "Generate database");
            Promt.Program.AddPage(pgGen);
            var op1 = new Option("Generate database", pgGen,() => Promt.Program.NavigateTo(pgGen));
            opts.Add(op1);
            Promt.Load();

            return opts.ToArray();

        }
        public MainPage()
            : base(Opts())
        {
        }
    }
}
