using System;
using Csud.Crud.DBTool.Promt.ConsoleEx;

namespace Csud.Crud.DBTool.Promt.Pages
{
    class StartDataBaseGeneration : Page
    {
        public StartDataBaseGeneration(ConsoleProgram consoleProgram)
            : base( consoleProgram)
        {
        }

        public override int Count { get; set; } = 0;

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
