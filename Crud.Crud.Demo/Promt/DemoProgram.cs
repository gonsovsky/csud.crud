using Csud.Crud.DBTool.Promt.EasyConsole;
using Csud.Crud.DBTool.Promt.Pages;

namespace Csud.Crud.DBTool.Promt
{
    public class DemoProgram : EasyConsole.Program
    {
        public DemoProgram()
            : base("EasyConsole Demo", breadcrumbHeader: true)
        {

            X.P = this;
            X.P.AddPage(new MainPage(X.P));
            MainPage.Opts();
 

            SetPage<MainPage>();
        }
    }
}
