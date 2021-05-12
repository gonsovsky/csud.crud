namespace Csud.Crud.DbTool.PromtEx.ConsoleEx
{
    public abstract class MenuPage : Page
    {
        protected Menu Menu { get; set; }

        public MenuPage(Option[] options)
            : base(null,"Csud.Crud")
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
