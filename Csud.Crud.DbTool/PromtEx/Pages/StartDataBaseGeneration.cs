using System;
using Csud.Crud.DbTool.PromtEx.ConsoleEx;

namespace Csud.Crud.DbTool.PromtEx.Pages
{
    class StartDataBaseGeneration : Page
    {
        public StartDataBaseGeneration(Type page, string title)
            : base(page, title)
        {
        }

        public override void Display()
        {
            base.Display();

            Output.WriteLine(ConsoleColor.Green,"Working...");

            Program.BeginGeneration();

            Input.ReadString("Press [Enter] to navigate home");
            ConsoleProgram.NavigateHome();
        }
    }
}
