using System;
using Csud.Crud.DbTool.PromtEx.ConsoleEx;

namespace Csud.Crud.DbTool.PromtEx.Pages
{
    public class BasePage: Page
    {
        public override void Display()
        {
            base.Display();

            int cnt = Input.ReadInt($"{PageType.TypeName()} (from 1 to 100.000): ", 1, 10000);
            Promt.Result.TypeSet(PageType, cnt);
            Promt.Save();
            ConsoleProgram.NavigateHome();
        }

        public BasePage(Type page, string title) : base(page, title)
        {
        }
    }
}
