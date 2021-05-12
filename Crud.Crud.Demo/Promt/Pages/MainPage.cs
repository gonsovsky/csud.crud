using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Csud.Crud.DBTool.Promt.EasyConsole;

namespace Csud.Crud.DBTool.Promt.Pages
{
    class MainPage : MenuPage
    {
        public static Option[] Opts()
        {
            var opts = new List<Option>();
            var x =  Assembly.GetExecutingAssembly().GetTypes()
                .Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(typeof(Base))).ToList();
            foreach (var q in x)
            {
                var p = (Page)Activator.CreateInstance(q, X.P);
                X.P.AddPage(p);
                var op = new Option(q.Name, () => X.P.NavigateTo2( q));
                opts.Add(op);
            }
            X.P.AddPage(new StartDataBaseGeneration(X.P));
            var op1 = new Option(typeof(StartDataBaseGeneration).Name, () => X.P.NavigateTo2(typeof(StartDataBaseGeneration)));
            opts.Add(op1);

            return opts.ToArray();

        }
        public MainPage(EasyConsole.Program program)
            : base(program, Opts())
        {
        }

        public override void Display()
        {
            base.Display();

        }

        public override int Count { get; set; } = 0;
    }
}
