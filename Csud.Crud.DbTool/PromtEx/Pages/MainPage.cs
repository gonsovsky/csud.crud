using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csud.Crud.DbTool.PromtEx.ConsoleEx;

namespace Csud.Crud.DbTool.PromtEx.Pages
{
    internal class MainPage: MenuPage
    {
        internal static  Option[] Opts()
        {
            var opts = new List<Option>();

            var op1 = new Option("Create database :", Promt.CreatePage, () => Promt.Program.NavigateTo(Promt.CreatePage));
            var op2 = new Option("Import database  : [Application... tables] from XML file", Promt.ImportPage, () => Promt.Program.NavigateTo(Promt.ImportPage));
            var op3 = new Option("Generate database: [All tables] with fake data", Promt.GenPage, () => Promt.Program.NavigateTo(Promt.GenPage));
            opts.Add(op1);
            opts.Add(op2);
            opts.Add(op3);
            Promt.Load();

            return opts.ToArray();
        }
        public MainPage()
            : base(Opts())
        {
        }
    }
}
