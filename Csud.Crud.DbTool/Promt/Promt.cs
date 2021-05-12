using Csud.Crud.DBTool.Promt.ConsoleEx;
using Csud.Crud.DBTool.Promt.Pages;

namespace Csud.Crud.DBTool.Promt
{
    public class Promt : ConsoleProgram
    {
        public Promt()
            : base("Crud.Csud.DBTool", breadcrumbHeader: true)
        {
            X.P = this;
            X.P.AddPage(new MainPage(X.P));
            MainPage.Opts();
            SetPage<MainPage>();
        }
    }
}
