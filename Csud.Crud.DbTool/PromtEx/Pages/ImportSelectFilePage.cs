using System;
using Csud.Crud.DbTool.PromtEx.ConsoleEx;

namespace Csud.Crud.DbTool.PromtEx.Pages
{
    internal class ImportSelectFilePage : Page
    {


        public override void Display()
        {
            base.Display();
            ImportPage.FileName = Input.ReadString($"Type file name: ");
            ConsoleProgram.NavigateBack();
        }

        public ImportSelectFilePage(Type page, string title) : base(page, title)
        {
        }
    }
}
