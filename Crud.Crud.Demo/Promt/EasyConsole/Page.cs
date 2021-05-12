using System;
using System.Linq;

namespace Csud.Crud.DBTool.Promt.EasyConsole
{
    public abstract class Page
    {
        public string Title { get; private set; }

        public abstract int Count { get; set; }

        public Program Program { get; set; }

        public Page(Program program)
        {
            Title = GetType().Name;
            Program = program;
        }

        public virtual void Display()
        {
            if (Program.History.Count > 1 && Program.BreadcrumbHeader)
            {
                string breadcrumb = null;
                foreach (var title in Program.History.Select((page) => page.Title).Reverse())
                    breadcrumb += title + " > ";
                breadcrumb = breadcrumb.Remove(breadcrumb.Length - 3);
                Console.WriteLine(breadcrumb);
            }
            else
            {
                Console.WriteLine(Title);
            }
            Console.WriteLine("---");
        }
    }
}
