using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;

namespace Csud.Crud
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
        public void Generate()
        {
            var r = new Random();
            var n = 0;
            using var sr = new StreamReader(DataFile);
            Dictionary<string, string> fields = null;
            string currentLine;
            var ap = new AccountProvider()
            {
                Description = "Active Directory",
                DisplayName = "Active Directory",
                Name = "ActiveDirectory"
            };
            Csud.Add(ap);
            while ((currentLine = sr.ReadLine()) != null)
            {
                n++;
                if (n == 200)
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
                        Status = ItemStatus.Actual,
                        Description = V(values, fields, "useraccountcontrol"),
                        Name = V(values, fields, "samaccountname"),
                    };
                    Csud.Add(p);

                    var su = new Subject()
                    {
                        SubjectType = Subject.SubjectTypeEnum.Account,
                        Description = "subject:" + V(values, fields, "useraccountcontrol"),
                        Name = "subject:" + V(values, fields, "samaccountname"),
                        DisplayName = "subject:" + V(values, fields, "userprincipalname"),
                    };
                    Csud.Add(su);

                    var ac = new Account()
                    {
                        AccountProvider = ap,
                        Description = V(values, fields, "useraccountcontrol"),
                        Name = V(values, fields, "samaccountname"),
                        DisplayName = V(values, fields, "userprincipalname"),
                        Person = p,
                        Subject = su
                    };
                    Csud.Add(ac);

                    Console.WriteLine(ac.ID);
                }
                else
                {
                    var co = new Context()
                    {
                        Description = "context:" + V(values, fields, "useraccountcontrol"),
                        Name = "context:" + V(values, fields, "samaccountname"),
                        DisplayName = "context:" + V(values, fields, "userprincipalname"),
                    };
                    Csud.Add(co);

                    var timeCo = new TimeContext()
                    {
                        Description = "timeContext:" + V(values, fields, "useraccountcontrol"),
                        Name = "timeContext:" + V(values, fields, "samaccountname"),
                        DisplayName = "timeContext:" + V(values, fields, "userprincipalname"),
                        Context = co,
                        TimeStart = new TimeSpan(r.Next(1, 23), r.Next(1, 59), r.Next(1, 59)),
                        TimeEnd = new TimeSpan(r.Next(1, 23), r.Next(1, 59), r.Next(1, 59)),
                    };
                    Csud.Add(timeCo);

                    var segmentContext = new SegmentContext()
                    {
                        Description = "segmentContext:" + V(values, fields, "useraccountcontrol"),
                        Name = "segmentContext:" + V(values, fields, "samaccountname"),
                        DisplayName = "segmentContext:" + V(values, fields, "userprincipalname"),
                        Context = co,
                    };
                    Csud.Add(segmentContext);

                    var structContext = new StructContext()
                    {
                        Description = "structContext:" + V(values, fields, "useraccountcontrol"),
                        Name = "structContext:" + V(values, fields, "samaccountname"),
                        DisplayName = "structContext:" + V(values, fields, "userprincipalname"),
                        Context = co,
                        StructCode = n.ToString()
                    };
                    Csud.Add(structContext);

                    var su = new Subject()
                    {
                        SubjectType = Subject.SubjectTypeEnum.Group,
                        Description = "subject:" + V(values, fields, "useraccountcontrol"),
                        Name = "subject:" + V(values, fields, "samaccountname"),
                        DisplayName = "subject:" + V(values, fields, "userprincipalname"),
                        Context = co
                    };
                    Csud.Add(su);
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
