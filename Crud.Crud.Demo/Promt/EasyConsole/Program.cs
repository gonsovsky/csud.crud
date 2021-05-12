using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Csud.Crud.DBTool.Promt.EasyConsole
{
    public abstract class Program
    {
        public static Config Cfg => DBTool.Program.Cfg;
        protected string Title { get; set; }

        public bool BreadcrumbHeader { get; private set; }

        protected Page CurrentPage
        {
            get
            {
                return (History.Any()) ? History.Peek() : null;
            }
        }

        private Dictionary<Type, Page> Pages { get; set; }

        public Stack<Page> History { get; private set; }

        public bool NavigationEnabled { get { return History.Count > 1; } }

        protected Program(string title, bool breadcrumbHeader)
        {
            Title = title;
            Pages = new Dictionary<Type, Page>();
            History = new Stack<Page>();
            BreadcrumbHeader = breadcrumbHeader;
        }

        public virtual void Run()
        {
            Output.WriteLine(ConsoleColor.White, $@"Csud.Crud database tool");

            var mongo = Cfg.Mongo.Enabled ? "" : "NOT"; ;
            var postgre = Cfg.Postgre.Enabled ? "" : "NOT"; ;

            Output.WriteLine(ConsoleColor.Yellow, $@"This will {mongo} generate the Mongo database {Cfg.Mongo.Host}:{Cfg.Mongo.Port} DB:{Cfg.Mongo.Db}.");
            Output.WriteLine(ConsoleColor.Yellow, $@"This will {postgre} generate the Postgre database.");

            Output.WriteLine("");
            Output.WriteLine(ConsoleColor.Yellow, $@"To enable/disable Mongo/Postgre and change settings, see app.config file.");
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

            Type pageType = page.GetType();

            X.Stat[pageType.Name] = page.Count;

            if (Pages.ContainsKey(pageType))
                Pages[pageType] = page;
            else
                Pages.Add(pageType, page);
        }

        public void NavigateHome()
        {
            while (History.Count > 1)
                History.Pop();

            Console.Clear();
            CurrentPage.Display();
        }

        public T SetPage<T>() where T : Page
        {
            Type pageType = typeof(T);

            if (CurrentPage != null && CurrentPage.GetType() == pageType)
                return CurrentPage as T;

            // leave the current page

            // select the new page
            Page nextPage;
            if (!Pages.TryGetValue(pageType, out nextPage))
                throw new KeyNotFoundException("The given page \"{0}\" was not present in the program".Format(pageType));

            // enter the new page
            History.Push(nextPage);

            return CurrentPage as T;
        }

        public T NavigateTo<T>() where T : Page
        {
            SetPage<T>();

            Console.Clear();
            CurrentPage.Display();
            return CurrentPage as T;
        }

        public Page NavigateTo2(Type page)
        {
            SetPage2(page);

            Console.Clear();
            CurrentPage.Display();
            return CurrentPage;
        }

        public Page SetPage2(Type page)
        {
            Type pageType = page;

            if (CurrentPage != null && CurrentPage.GetType() == pageType)
                return CurrentPage;

            // leave the current page

            // select the new page
            Page nextPage;
            if (!Pages.TryGetValue(pageType, out nextPage))
                throw new KeyNotFoundException("The given page \"{0}\" was not present in the program".Format(pageType));

            // enter the new page
            History.Push(nextPage);

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
