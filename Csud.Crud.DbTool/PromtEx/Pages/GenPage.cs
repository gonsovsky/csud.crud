using System.Collections.Generic;
using System.Linq;
using Csud.Crud.DbTool.PromtEx.ConsoleEx;
using Csud.Crud.Models.Internal;

namespace Csud.Crud.DbTool.PromtEx.Pages
{
    internal class GenPage : MenuPage
    {
        internal static Option[] Opts()
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
            var pgGen  = new GenPageWork(null, "Generate database");
            Promt.Program.AddPage(pgGen);
            var op1 = new Option("Generate database", pgGen,() => Promt.Program.NavigateTo(pgGen));
            opts.Add(op1);
            Promt.Load();

            return opts.ToArray();
        }
        public GenPage()
            : base(Opts())
        {
        }
    }
}
