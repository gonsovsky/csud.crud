using System;
using System.Collections.Generic;
using System.Linq;

namespace Csud.Crud.DBTool.Promt.ConsoleEx
{
    public class Menu
    {
        private IList<Option> Options { get; set; }

        public Menu()
        {
            Options = new List<Option>();
        }

        public void Display()
        {
            for (int i = 0; i < Options.Count; i++)
            {
                var cnt = 0;
                if (X.Stat.ContainsKey(Options[i].Name))
                    cnt = X.Stat[Options[i].Name];
                if (Options[i].Name.Contains($"StartDataBaseGeneration"))
                {
                    Output.WriteLine("");
                    Output.WriteLine(ConsoleColor.Green, "{0}. {1}", i + 1, Options[i].Name + " (" + ConsoleProgram.Cfg.UseDataBase +")");
                }
                else
                {
                    if (cnt == 0)
                        Console.WriteLine("{0}. {1}", i + 1, Options[i].Name);
                    else Console.WriteLine("{0}. {1}", i + 1, Options[i].Name + ": " + cnt);
                }
                    
            }
            Console.WriteLine();
            int choice = Input.ReadInt("Choose an option:", min: 1, max: Options.Count);

            Options[choice - 1].Callback();
        }

        public Menu Add(string option, Action callback)
        {
            return Add(new Option(option, callback));
        }

        public Menu Add(Option option)
        {
            Options.Add(option);
            return this;
        }

        public bool Contains(string option)
        {
            return Options.FirstOrDefault((op) => op.Name.Equals(option)) != null;
        }
    }
}
