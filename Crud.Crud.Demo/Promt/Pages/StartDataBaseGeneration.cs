using System;
using Csud.Crud.DBTool.Promt.EasyConsole;

namespace Csud.Crud.DBTool.Promt.Pages
{
    class StartDataBaseGeneration : Page
    {
        public StartDataBaseGeneration(EasyConsole.Program program)
            : base( program)
        {
        }

        public override int Count { get; set; } = 0;

        public override void Display()
        {
            base.Display();

            Output.WriteLine(ConsoleColor.Green,"Working...");

            DBTool.Program.BeginGeneration();

            Input.ReadString("Press [Enter] to navigate home");
            Program.NavigateHome();
        }
    }
}
