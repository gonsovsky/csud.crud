using System;
using System.Linq;

namespace Csud.Crud.DbTool.PromtEx.ConsoleEx
{
    public class Page
    {
        public string Title { get; private set; }

        public Type PageType;

        public ConsoleProgram ConsoleProgram { get; set; }

        public Page(Type page, string title)
        {
            ConsoleProgram = Promt.Program;
            if (page != null)
                Title = page.Name;
            else
                Title = title;
            PageType = page;
        }

        public virtual void Display()
        {
            if (ConsoleProgram.History.Count > 1 && ConsoleProgram.BreadcrumbHeader)
            {
                string breadcrumb = null;
                foreach (var title in ConsoleProgram.History.Select((page) => page.Title).Reverse())
                    breadcrumb += title + " > ";
                breadcrumb = breadcrumb.Remove(breadcrumb.Length - 3);
                Console.WriteLine(breadcrumb);
            }
            else
            {
                Console.WriteLine(Title);
            }
            Console.WriteLine("");
        }
    }
}
