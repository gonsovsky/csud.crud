﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Models.Rules;
using Group = Csud.Crud.Models.Rules.Group;

namespace Csud.Crud.Demo
{
    public class DataGenerator
    {
        protected ICsud Csud;
        public DataGenerator(ICsud csud)
        {
            Csud = csud;
        }
        protected string V(string[] v, Dictionary<string, string> f, string name, int part=-1)
        {
            var s = v[f.Keys.ToList().IndexOf(name)].Replace("\"", "");
            if (part < 0) return s;
            try
            {
                return s.Split(' ')[part];
            }
            catch (Exception)
            {
                return s;
            }
        }
        public void Generate(int numbber)
        {
            CompositeContext LastContext=null;
            Group LastGroup = null;
            var r = new Random();
            var n = 0;
            using var sr = new StreamReader(DataFile);
            Dictionary<string, string> fields = null;
            string currentLine;
            var ap = new AccountProvider()
            {
                Description = "Active Directory",
                DisplayName = "Active Directory",
                Name = "ActiveDirectory",
                Type = "Active Directory"
            };
            Csud.Insert(ap);

            while ((currentLine = sr.ReadLine()) != null)
            {
                n++;
                if (n == numbber)
                    break;
                var values = Regex.Split(currentLine, ",(?=(?:[^']*'[^']*')*[^']*$)");
                if (fields == null)
                {
                    fields = values.Select((value, index) => new {value, index})
                        .ToDictionary(pair => pair.value, pair => pair.index.ToString());
                    continue;
                }

                //for (int i = 0; i <= fields.Count - 1; i++)
                //    values[i] = fields.Keys.ToList().ElementAt(i) + "  --" + values[i];

                var recType = V(values, fields, "structuralobjectclass");
                if (recType.Contains("user"))
                {
                    var p = new Person()
                    {
                        DisplayName = V(values, fields, "userprincipalname"),
                        FirstName = V(values, fields, "name",1),
                        LastName = V(values, fields, "name", 0),
                        Description = V(values, fields, "useraccountcontrol"),
                        Name = V(values, fields, "samaccountname"),
                    };
                    Csud.Insert(p);

                    var su = new Subject()
                    {
                        Description = "subject:" + V(values, fields, "useraccountcontrol"),
                        Name = "subject:" + V(values, fields, "samaccountname"),
                        DisplayName = "subject:" + V(values, fields, "userprincipalname"),
                        ContextKey = LastContext.Key
                    };
                    Csud.Insert(su);

                    var ac = new Account()
                    {
                        AccountProviderKey = 1,
                        Description = V(values, fields, "useraccountcontrol"),
                        Name = V(values, fields, "samaccountname"),
                        DisplayName = V(values, fields, "userprincipalname"),
                        Person = p,
                        Subject = su
                    };
                    Csud.Insert(ac);

                    var gr = (Group)LastGroup.Clone(false);
                    gr.Key = LastGroup.Key;
                    gr.RelatedKey = su.Key;
                    gr.ContextKey = LastGroup.ContextKey;
                    Csud.Insert(gr, false);

                    Console.WriteLine(ac.Key);
                }
                else
                {
                    var timeContext = new TimeContext()
                    {
                        Description = "timeContext:" + V(values, fields, "useraccountcontrol"),
                        Name = "timeContext:" + V(values, fields, "samaccountname"),
                        DisplayName = "timeContext:" + V(values, fields, "userprincipalname"),
                        TimeStart = new TimeSpan(r.Next(1, 23), r.Next(1, 59), r.Next(1, 59)).Ticks,
                        TimeEnd = new TimeSpan(r.Next(1, 23), r.Next(1, 59), r.Next(1, 59)).Ticks
                    };
                    Csud.AddContext(timeContext);

                    var segmentContext = new SegmentContext()
                    {
                        Description = "segmentContext:" + V(values, fields, "useraccountcontrol"),
                        Name = "segmentContext:" + V(values, fields, "samaccountname"),
                        DisplayName = "segmentContext:" + V(values, fields, "userprincipalname"),
                        SegmentName = "segment N " + n.ToString()
                    };
                    Csud.AddContext(segmentContext);

                    var structContext = new StructContext()
                    {
                        Description = "structContext:" + V(values, fields, "useraccountcontrol"),
                        Name = "structContext:" + V(values, fields, "samaccountname"),
                        DisplayName = "structContext:" + V(values, fields, "userprincipalname"),
                        StructCode = n.ToString()
                    };
                    Csud.AddContext(structContext);

                    var ruleContext = new RuleContext()
                    {
                        Description = "ruleContext:" + V(values, fields, "useraccountcontrol"),
                        Name = "ruleContext:" + V(values, fields, "samaccountname"),
                        DisplayName = "ruleContext:" + V(values, fields, "userprincipalname"),
                        RuleName = "rule N " + n.ToString()
                    };
                    Csud.AddContext(ruleContext);

                    var compositeContext = new CompositeContext()
                    {
                        Description = "compositeContext:" + V(values, fields, "useraccountcontrol"),
                        Name = "compositeContext:" + V(values, fields, "samaccountname"),
                        DisplayName = "compositeContext:" + V(values, fields, "userprincipalname"),
                    };

                    compositeContext.RelatedKeys.Add(timeContext.Key);
                    compositeContext.RelatedKeys.Add(segmentContext.Key);
                    compositeContext.RelatedKeys.Add(structContext.Key);
                    compositeContext.RelatedKeys.Add(ruleContext.Key);

                    Csud.AddContext(compositeContext);
                    LastContext = compositeContext;

                    var su = new Subject()
                    {
                        SubjectType = Const.Subject.Group,
                        Description = "subject:" + V(values, fields, "useraccountcontrol"),
                        Name = "subject:" + V(values, fields, "samaccountname"),
                        DisplayName = "subject:" + V(values, fields, "userprincipalname"),
                        ContextKey = LastContext.Key
                    };
                    Csud.Insert(su);

                    var gr = new Group()
                    {
                        Key = su.Key,
                        Name = "group:" + V(values, fields, "samaccountname"),
                        ContextKey = LastContext.Key
                    };
                    LastGroup = gr;
                    Console.WriteLine("group:" + gr.Key);

                    var obj1 = new ObjectX()
                    {
                        Type = Const.Object.Task,
                        Description = "object:" + V(values, fields, "useraccountcontrol"),
                        Name = "object:" + V(values, fields, "samaccountname"),
                        DisplayName = "object:" + V(values, fields, "userprincipalname"),
                        ContextKey = LastContext.Key
                    };
                    Csud.Insert(obj1);
                    var obj2 = (ObjectX)obj1.Clone();
                    Csud.Insert(obj2);

                    var task = new TaskX()
                    {
                        Key = obj1.Key,
                        RelatedKey = obj2.Key
                    };
                    Csud.Insert(task);
                }
            }
        }

        protected static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().Location;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        protected static string DataFile
        {
            get
            {
                var file = Path.Join(AssemblyDirectory, "../../../../data/scsm.csv");
                return file;
            }
        }

    }
}
