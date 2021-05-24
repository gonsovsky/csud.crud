using System.Collections.Generic;
using System.Linq;
using Csud.Crud.DbTool.PromtEx.ConsoleEx;
using Csud.Crud.Services;

namespace Csud.Crud.DbTool.PromtEx.Pages
{
    class MainPage : MenuPage
    {
        public static Option[] Opts()
        {
            var opts = new List<Option>();
       
            foreach (var type in Promt.Types)
            {
                if (!type.GetInterfaces().Contains(typeof(INoneRepo)))
                {
                    var p = new BasePage(type, type.Name);
                    Promt.Program.AddPage(p);
                    var op = new Option(type.Name, p, () => Promt.Program.NavigateTo(p));
                    opts.Add(op);
                }
                if (!Promt.Result.TypeHas(type))
                    Promt.Result.TypeSet(type, 20);
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
