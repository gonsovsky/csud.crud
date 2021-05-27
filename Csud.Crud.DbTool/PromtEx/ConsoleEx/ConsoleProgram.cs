using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Csud.Crud.DbTool.PromtEx.ConsoleEx
{
    public abstract class ConsoleProgram
    {
        public static Config Cfg => Tool.Cfg;
        protected string Title { get; set; }

        public bool BreadcrumbHeader { get; private set; }

        protected Page CurrentPage
        {
            get
            {
                return (History.Any()) ? History.Peek() : null;
            }
        }

        private List<Page> Pages { get; set; }

        public Stack<Page> History { get; private set; }

        public bool NavigationEnabled { get { return History.Count > 1; } }

        protected ConsoleProgram(string title, bool breadcrumbHeader)
        {
            Title = title;
            Pages = new List<Page>();
            History = new Stack<Page>();
            BreadcrumbHeader = breadcrumbHeader;
        }

        public virtual void Run()
        {
            Output.WriteLine(ConsoleColor.White, $@"Csud.Crud database tool");

            var mongo = Cfg.Mongo.Enabled ? " " : "NOT"; ;
            var postgre = Cfg.Postgre.Enabled ? " " : "NOT"; ;

            Output.WriteLine(ConsoleColor.Yellow, $@"This will {mongo}generate the Mongo database {Cfg.Mongo.Host}:{Cfg.Mongo.Port} DB:{Cfg.Mongo.Db}.");
            Output.WriteLine(ConsoleColor.Yellow, $@"This will {postgre}generate the Postgre database {Cfg.Postgre.Host}:{Cfg.Postgre.Port} DB:{Cfg.Postgre.Db}.");

            Output.WriteLine("");
            Output.WriteLine(ConsoleColor.Yellow, $@"To enable/disable Mongo/Postgre and change settings, see appsettings.json file.");
            Output.WriteLine(ConsoleColor.Yellow, $@"");

            try
            {
                Console.Title = Title;

                CurrentPage.Display();
            }
            catch (Exception e)
            {
                Output.WriteLine(ConsoleColor.Red, e.ToString());
            }
            finally
            {
                if (Debugger.IsAttached)
                {
                    Input.ReadString("Press [Enter] to exit");
                }
            }
        }

        public void AddPage(Page page)
        {
            if (!Pages.Contains(page))
                Pages.Add(page);
        }

        public void NavigateHome()
        {
            while (History.Count > 1)
                History.Pop();

            Console.Clear();
            CurrentPage.Display();
        }

        public Page NavigateTo(Page page)
        {
            SetPage(page);

            Console.Clear();
            CurrentPage.Display();
            return CurrentPage;
        }

        public Page SetPage(Page page)
        {
            History.Push(page);

            return CurrentPage;
        }

        public Page NavigateBack()
        {
            History.Pop();
            Console.Clear();
            CurrentPage.Display();
            return CurrentPage;
        }
    }
}
