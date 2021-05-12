using System;
using System.Linq;

namespace Csud.Crud.DBTool.Promt.ConsoleEx
{
    public abstract class Page
    {
        public string Title { get; private set; }

        public abstract int Count { get; set; }

        public ConsoleProgram ConsoleProgram { get; set; }

        public Page(ConsoleProgram consoleProgram)
        {
            Title = GetType().Name;
            ConsoleProgram = consoleProgram;
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
            Console.WriteLine("---");
        }
    }
}
