using System;
using Csud.Crud.DbTool.PromtEx.ConsoleEx;

namespace Csud.Crud.DbTool.PromtEx.Pages
{
    internal class GenPageWork : Page
    {
        public GenPageWork(Type page, string title)
            : base(page, title)
        {
        }

        public override void Display()
        {
            base.Display();

            Output.WriteLine(ConsoleColor.Green,"Generating DB...");

            Program.Generate();

            Input.ReadString("Press [Enter] to navigate home");
            ConsoleProgram.NavigateHome();
        }
    }
}
