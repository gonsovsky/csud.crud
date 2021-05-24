using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Csud.Crud.DbTool.PromtEx.ConsoleEx;
using Csud.Crud.DbTool.PromtEx.Pages;
using Csud.Crud.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Csud.Crud.DbTool.PromtEx
{
    public class Promt : ConsoleProgram
    {
        public Promt()
            : base("Csud.Crud.DBTool", breadcrumbHeader: true)
        {
            Program = this;
            var mainPage = new MainPage();
            Program.AddPage(mainPage);
            SetPage(mainPage);
        }

        public static ConsoleProgram Program;

        public static Dictionary<Type, int> Result = new();

        public static void Save()
        {
            try
            {
                var save = Result.Select(x => new {x.Key.Name, x.Value});
                var json = JsonSerializer.Serialize(save);
                File.WriteAllText(Path.Combine(AssemblyDirectory, "q.txt"), json);
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
                var json = File.ReadAllText(Path.Combine(AssemblyDirectory, "q.txt"));
                var obj = JsonSerializer.Deserialize<List<Q>>(json);
                var types = Types.ToArray();
                foreach (var x in obj)
                {
                    try
                    {
                        Result[types.First(a => a.Name == x.Name)] = x.Value;
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
                        if (theType.IsClass  && !theType.IsAbstract && theType.IsSubclassOf(typeof(Base)))
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

    public static class P
    {

        public static int TypeGet(this Dictionary<Type, int> res, Type type)
        {
            var x = res.First(a => TypeName(a.Key) == TypeName(type)).Key;
            return res[x];
        }

        public static string TypeName(this Type type)
        {
            if (type.Name.EndsWith("Add"))
                return type.Name.Substring(0, type.Name.LastIndexOf("Add"));
            if (type.Name.EndsWith("Edit"))
                return type.Name.Substring(0, type.Name.LastIndexOf("Edit"));
            return type.Name;
        }

        public static bool TypeHas(this Dictionary<Type, int> res, Type type)
        {
            return res.Any(x => TypeName(x.Key) == TypeName(type));
        }

        public static void TypeSet(this Dictionary<Type, int> res, Type type, int value)
        {
            if (TypeHas(res, type))
            {
                var x = res.First(a => TypeName(a.Key) == TypeName(type)).Key;
                res[x] = value;
            }
            else
            {
                res.Add(type, value);
            }
        }
    }
}
