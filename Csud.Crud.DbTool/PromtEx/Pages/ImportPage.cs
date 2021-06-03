using System.Collections.Generic;
using Csud.Crud.DbTool.PromtEx.ConsoleEx;

namespace Csud.Crud.DbTool.PromtEx.Pages
{
    internal class ImportPage : MenuPage
    {
        public static string FileName { get; set; }

        internal static string DefFile = "./Resources/КПИ_ИС.xml";
        internal static Option[] Opts()
        {
            FileName = DefFile;
            var opts = new List<Option>();

            var p1 = new ImportSelectFilePage(null, "Select XML file");
            Promt.Program.AddPage(p1);
            var op1 = new Option("Select XML file", p1, () => Promt.Program.NavigateTo(p1));
            opts.Add(op1);


            var p2 = new ImportPageWork(null, "Imorting XML file");
            Promt.Program.AddPage(p2);
            var op2 = new Option("Start Import", p2, () => Promt.Program.NavigateTo(p2));
            opts.Add(op2);

            return opts.ToArray();
        }

        public ImportPage()
            : base(Opts())
        {
        }
    }
}
