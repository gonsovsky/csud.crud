using Csud.Crud.DBTool.Promt.EasyConsole;

namespace Csud.Crud.DBTool.Promt.Pages
{
    abstract class Base : Page
    {


        public Base(EasyConsole.Program program)
            : base(program)
        {
        }



        public override void Display()
        {
            base.Display();

            int cnt = Input.ReadInt($"{GetType().Name} (from 1 to 100.000): ",1,10000);
            X.Stat[GetType().Name] = cnt;

         //   Output.WriteLine(ConsoleColor.Green, "You selected {0}", input);

           // Input.ReadString("Press [Enter] to navigate home");
            Program.NavigateHome();
        }
    }

    enum Fruit
    {
        Apple,
        Banana,
        Coconut
    }
}
