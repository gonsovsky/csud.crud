using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csud.Crud.DbTool.PromtEx.ConsoleEx;

namespace Csud.Crud.DbTool.PromtEx.Pages
{
    internal class ImportPageWork : Page
    {
        public ImportPageWork(Type page, string title)
            : base(page, title)
        {
        }

        public override void Display()
        {
            base.Display();

            Output.WriteLine(ConsoleColor.Green, $"Importing XML ({ImportPage.FileName})...");

            Program.Import(ImportPage.FileName);

            Input.ReadString("Press [Enter] to navigate home");
            ConsoleProgram.NavigateHome();
        }
    }
}
