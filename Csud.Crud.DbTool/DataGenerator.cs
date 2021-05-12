using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Reflection;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Models.Rules;

namespace Csud.Crud.DbTool
{
    public class DataGenerator
    {
        protected ICsud Csud;
        public DataGenerator(ICsud csud)
        {
            Csud = csud;
        }

        private Dictionary<Type, int> input;

        private Dictionary<Type, int> result;

        private int From<T>() where T : Base
        {
            return result[typeof(T)];
        }

        private int To<T>() where T : Base
        {
            return input[typeof(T)];
        }
        private bool Ready<T>() where T : Base
        {
            return input[typeof(T)] <= result[typeof(T)]-1;
        }

        private readonly Random r = new Random();

        private T Gen<T>() where T : Base
        {
            var source = Activator.CreateInstance<T>();
            var typeSrc = source.GetType();
            var results = typeSrc.GetProperties()
                .Where(srcProp => srcProp.CanRead && srcProp.GetSetMethod(true) != null 
                                                  && !srcProp.GetSetMethod(true).IsPrivate &&
                                                  (srcProp.GetSetMethod().Attributes & MethodAttributes.Static) == 0 &&
                                                  srcProp.PropertyType.IsAssignableFrom(typeof(string)))
                .Select(srcProp => new {sourceProperty = srcProp});
            foreach (var props in results)
            {
                props.sourceProperty.SetValue(source, $"{props.sourceProperty.Name} - {From<T>()}");
            }
            source.Status = Const.Status.Actual;
            return source;
        }

        private void Log<T>() where T : Base
        {
            var s = $@"{typeof(T).Name} - {From<T>()}/{To<T>()}";
            Console.WriteLine(s);
            result[typeof(T)] += 1;
        }

        private void Make<T>(Action<T> act = null) where T : Base
        {
            var a = Gen<T>();
            if (act != null) act(a);
            Csud.AddEntity(a);
            Log<T>();
        }

        private void MakeContext<T>(Action<T> act = null) where T : BaseContext
        {
            var a = Gen<T>();
            act?.Invoke(a);
            Csud.AddContext(a);
            Log<T>();
        }

        private void MakeTimeContext(TimeContext a)
        {
            a.TimeStart = new TimeSpan(r.Next(1, 23), r.Next(1, 59), r.Next(1, 59)).Ticks;
            a.TimeEnd = new TimeSpan(r.Next(1, 23), r.Next(1, 59), r.Next(1, 59)).Ticks;
        }

        private void MakeCompositeContext(CompositeContext a)
        {
            a.RelatedKeys = Csud.Context.Where(x => x.ContextType != Const.Context.Composite)
                .Select(x => x.Key).OrderBy(x => SqlFunctions.Rand()).Take(5).ToList();
        }

        public void Generate(Dictionary<Type, int> dict)
        {
            input = dict;
            result = new Dictionary<Type, int>();
            foreach (var p in dict)
                result[p.Key] = 1;

            while (!Ready<AccountProvider>())
                Make<AccountProvider>();

            while (!Ready<Person>())
                Make<Person>();

            while (!Ready<TimeContext>())
                MakeContext<TimeContext>(MakeTimeContext);

            while (!Ready<SegmentContext>())
                MakeContext<SegmentContext>();

            while (!Ready<StructContext>())
                MakeContext<StructContext>();

            while (!Ready<RuleContext>())
                MakeContext<RuleContext>();

            while (!Ready<CompositeContext>())
                MakeContext<CompositeContext>(MakeCompositeContext);

            //while ((currentLine = sr.ReadLine()) != null)
            //{
            //    n++;
            //    var values = Regex.Split(currentLine, ",(?=(?:[^']*'[^']*')*[^']*$)");
            //    if (fields == null)
            //    {
            //        fields = values.Select((value, index) => new {value, index})
            //            .ToDictionary(pair => pair.value, pair => pair.index.ToString());
            //        continue;
            //    }


            //    var recType = V(values, fields, "structuralobjectclass");
            //    if (recType.Contains("user"))
            //    {
            //        var p = new Person()
            //        {
            //            FirstName = V(values, fields, "name",1),
            //            LastName = V(values, fields, "name", 0),
            //        };
            //        Csud.AddEntity(p);

            //        var su = new Subject()
            //        {
            //            Description = "subject:" + V(values, fields, "useraccountcontrol"),
            //            Name = "subject:" + V(values, fields, "samaccountname"),
            //            DisplayName = "subject:" + V(values, fields, "userprincipalname"),
            //            ContextKey = LastContext.Key
            //        };
            //        Csud.AddEntity(su);

            //        var ac = new Account()
            //        {
            //            AccountProviderKey = 1,
            //            Description = V(values, fields, "useraccountcontrol"),
            //            Name = V(values, fields, "samaccountname"),
            //            DisplayName = V(values, fields, "userprincipalname"),
            //            Person = p,
            //            Subject = su
            //        };
            //        Csud.AddEntity(ac);

            //        var gr = (Group)LastGroup.Clone(false);
            //        gr.Key = LastGroup.Key;
            //        gr.RelatedKey = su.Key;
            //        gr.ContextKey = LastGroup.ContextKey;
            //        Csud.AddEntity(gr, false);

            //        Console.WriteLine(ac.Key);
            //    }
            //    else
            //    {
            //        var timeContext = new TimeContext()
            //        {
            //            TimeStart = new TimeSpan(r.Next(1, 23), r.Next(1, 59), r.Next(1, 59)).Ticks,
            //            TimeEnd = new TimeSpan(r.Next(1, 23), r.Next(1, 59), r.Next(1, 59)).Ticks
            //        };
            //        Csud.AddContext(timeContext);

            //        var segmentContext = new SegmentContext()
            //        {
            //            SegmentName = "segment N " + n.ToString()
            //        };
            //        Csud.AddContext(segmentContext);

            //        var structContext = new StructContext()
            //        {
            //            StructCode = n.ToString()
            //        };
            //        Csud.AddContext(structContext);

            //        var ruleContext = new RuleContext()
            //        {
            //            RuleName = "rule N " + n.ToString()
            //        };
            //        Csud.AddContext(ruleContext);

            //        var compositeContext = new CompositeContext()
            //        {
            //        };

            //        compositeContext.RelatedKeys.Add(timeContext.Key);
            //        compositeContext.RelatedKeys.Add(segmentContext.Key);
            //        compositeContext.RelatedKeys.Add(structContext.Key);
            //        compositeContext.RelatedKeys.Add(ruleContext.Key);

            //        Csud.AddContext(compositeContext);
            //        LastContext = compositeContext;

            //        var su = new Subject()
            //        {
            //            SubjectType = Const.Subject.Group,
            //            Description = "subject:" + V(values, fields, "useraccountcontrol"),
            //            Name = "subject:" + V(values, fields, "samaccountname"),
            //            DisplayName = "subject:" + V(values, fields, "userprincipalname"),
            //            ContextKey = LastContext.Key
            //        };
            //        Csud.AddEntity(su);

            //        var gr = new Group()
            //        {
            //            Key = su.Key,
            //            ContextKey = LastContext.Key
            //        };
            //        LastGroup = gr;
            //        Console.WriteLine("group:" + gr.Key);

            //        var obj1 = new ObjectX()
            //        {
            //            Type = Const.Object.Task,
            //            Description = "object:" + V(values, fields, "useraccountcontrol"),
            //            Name = "object:" + V(values, fields, "samaccountname"),
            //            DisplayName = "object:" + V(values, fields, "userprincipalname"),
            //            ContextKey = LastContext.Key
            //        };
            //        Csud.AddEntity(obj1);
            //        var obj2 = (ObjectX)obj1.Clone();
            //        Csud.AddEntity(obj2);

            //        var task = new TaskX()
            //        {
            //            Key = obj1.Key,
            //            RelatedKey = obj2.Key
            //        };
            //        Csud.AddEntity(task);
            //    }
            //}
        }
    }
}
