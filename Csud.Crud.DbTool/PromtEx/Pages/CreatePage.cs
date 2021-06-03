using System;
using Csud.Crud.DbTool.PromtEx.ConsoleEx;

namespace Csud.Crud.DbTool.PromtEx.Pages
{
    internal class CreatePage : Page
    {
        public CreatePage()
            : base(null,"Create database")
        {
        }

        public override void Display()
        {
            base.Display();

            Output.WriteLine(ConsoleColor.Green, $"Creating database...");

            Program.Create();

            Output.WriteLine(ConsoleColor.Yellow, "Database created.");
            Output.WriteLine("");

            Input.ReadString("Press [Enter] to navigate home");
            ConsoleProgram.NavigateHome();
        }
    }
}