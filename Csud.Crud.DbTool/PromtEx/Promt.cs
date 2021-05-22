using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Csud.Crud.DbTool.PromtEx.ConsoleEx;
using Csud.Crud.DbTool.PromtEx.Pages;
using Csud.Crud.Models;
using Csud.Crud.Services;

namespace Csud.Crud.DbTool.PromtEx
{
    public class Promt : ConsoleProgram
    {
        public Promt()
            : base("Csud.Crud.DBTool", breadcrumbHeader: true)
        {
            Promt.Program = this;
            var mainPage = new MainPage();
            Promt.Program.AddPage(mainPage);
            SetPage(mainPage);
        }

        public static ConsoleProgram Program;

        public static Dictionary<Type, int> Result = new Dictionary<Type, int>();

        public static void Save()
        {
            try
            {
                var save = Result.Select(x => new {x.Key.Name, x.Value});
                var json = JsonSerializer.Serialize(save);
                System.IO.File.WriteAllText(Path.Combine(AssemblyDirectory, "q.txt"), json);
            }
            catch
            {
                // ignored
            }
        }

        class Q
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        public static void Load()
        {
            try
            {
                var json = System.IO.File.ReadAllText(Path.Combine(AssemblyDirectory, "q.txt"));
                var obj = JsonSerializer.Deserialize<List<Q>>(json);
                foreach (var x in obj)
                {
                    try
                    {
                        Result[Types.First(a => a.Name == x.Name)] = x.Value;
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public static IEnumerable<Type> Types
        {
            get
            {
                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var theType in a.GetTypes())
                    {
                        if (theType.IsClass && !theType.GetInterfaces().Contains(typeof(INoneRepo)) && !theType.IsAbstract && theType.IsSubclassOf(typeof(Base)))
                        {
                            yield return theType;
                        }
                    }
                }

            }
        }

        public static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
