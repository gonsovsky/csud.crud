namespace Csud.Crud.DBTool.Promt.ConsoleEx
{
    public abstract class MenuPage : Page
    {
        protected Menu Menu { get; set; }

        public MenuPage(ConsoleProgram consoleProgram, Option[] options)
            : base(consoleProgram)
        {
            Menu = new Menu();

            foreach (var option in options)
                Menu.Add(option);
        }

        public override void Display()
        {
            base.Display();

            if (ConsoleProgram.NavigationEnabled && !Menu.Contains("Go back"))
                Menu.Add("Go back", () => { ConsoleProgram.NavigateBack(); });

            Menu.Display();
        }
    }
}
