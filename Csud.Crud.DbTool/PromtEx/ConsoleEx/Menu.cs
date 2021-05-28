using System;
using System.Collections.Generic;
using System.Linq;
using Csud.Crud.DbTool.PromtEx.Pages;

namespace Csud.Crud.DbTool.PromtEx.ConsoleEx
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
                if (Options[i].Page is {PageType: { }}) cnt = Promt.Result.TypeGet(Options[i].Page.PageType);

                if (Options[i].Name.Contains($"Select XML file"))
                {
                    Output.WriteLine("");
                    Output.WriteLine(ConsoleColor.Green, "{0}. {1}", i + 1, Options[i].Name + " [selected: " + ImportPage.FileName + "]");
                }

                else if (Options[i].Name.Contains($"Start Import"))
                {
                    Output.WriteLine("");
                    Output.WriteLine(ConsoleColor.Magenta, "{0}. {1}", i + 1, Options[i].Name);
                    Output.WriteLine("");

                }

                else if (Options[i].Name.Contains($"Create database"))
                {
                    Output.WriteLine("");
                    Output.WriteLine(ConsoleColor.Green, "{0}. {1}", i + 1, Options[i].Name + " (" + ConsoleProgram.Cfg.UseDataBase + ")");
                }

                else if (Options[i].Name.Contains($"Generate database"))
                {
                    Output.WriteLine("");
                    Output.WriteLine(ConsoleColor.Green, "{0}. {1}", i + 1, Options[i].Name + " (" + ConsoleProgram.Cfg.UseDataBase +")");
                }
                else if (Options[i].Name.Contains($"Import database"))
                {
                    Output.WriteLine("");
                    Output.WriteLine(ConsoleColor.Green, "{0}. {1}", i + 1, Options[i].Name + " (" + ConsoleProgram.Cfg.UseDataBase + ")");
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
            return Add(new Option(option,null, callback));
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
