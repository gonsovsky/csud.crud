using Csud.Crud.DBTool.Promt.ConsoleEx;

namespace Csud.Crud.DBTool.Promt.Pages
{
    abstract class Base : Page
    {


        public Base(ConsoleProgram consoleProgram)
            : base(consoleProgram)
        {
        }



        public override void Display()
        {
            base.Display();

            int cnt = Input.ReadInt($"{GetType().Name} (from 1 to 100.000): ",1,10000);
            X.Stat[GetType().Name] = cnt;

         //   Output.WriteLine(ConsoleColor.Green, "You selected {0}", input);

           // Input.ReadString("Press [Enter] to navigate home");
            ConsoleProgram.NavigateHome();
        }
    }

    enum Fruit
    {
        Apple,
        Banana,
        Coconut
    }
}
