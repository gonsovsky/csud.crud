using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Reflection;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Models.Rules;
using MongoDB.Driver.Core.WireProtocol.Messages;

namespace Csud.Crud.DbTool
{
    public class DataGenerator
    {
        protected int No;

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

        private Dictionary<Type, int> current;

        private IEnumerable<T> Take<T>(IQueryable<T> q, int number) where T : Base
        {
            var result = q.Skip(current[typeof(T)]).Take(number).ToList();
            var cnt = this.result.Count();
            var no = result.Count();
            if (no != number)
                current[typeof(T)] = 0;
            else
            {
                current[typeof(T)] += no;
                if (current[typeof(T)] >= cnt)
                    current[typeof(T)] = 0;
            }
            return result;
        }

        private void Log<T>() where T : Base
        {
            var s = $@"{typeof(T).Name} - {From<T>()}/{To<T>()}";
            Console.WriteLine(s);
            result[typeof(T)] += 1;
            No += 1;
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

        private void MakeRelative<T,T1>(Action<T> act = null) where T : Base, IRelational where T1 : Base
        {
            var a = Gen<T>();
            act?.Invoke(a);
            Csud.AddRelational<T,T1>(a);
            Log<T>();
        }

        private void MakeTimeContext(TimeContext a)
        {
            a.TimeStart = 1000000 + No * 1000000;
            a.TimeEnd = 1000000 + (No+1) * 1000000;
        }

        private void MakeCompositeContext(CompositeContext a)
        {
            var q = Csud.Context.Where(x => x.ContextType != Const.Context.Composite);
            a.RelatedKeys = Take(q, 3).Select(a => a.Key).ToList();
        }

        private void MakeSubject(Subject a)
        {
            a.ContextKey = Take(Csud.Context, 1).First().Key;
            a.SubjectType = Const.Subject.Account;
        }

        private void MakeAccount(Account a)
        {
            a.AccountProviderKey = Take(Csud.AccountProvider, 1).First().Key;
            a.SubjectKey = Take(Csud.Subject, 1).First().Key;
            a.Person = Take(Csud.Person, 1).First();
        }
        private void MakeObject(ObjectX a)
        {
            a.Context = Take(Csud.Context, 1).First();
            a.ObjectType = Const.Object.Entity;
        }

        private void MakeTask(TaskX a)
        {
            var q = Csud.Object.Where(x => x.ObjectType != Const.Object.Task);
            a.RelatedKeys = Take(q, 3).Select(a => a.Key).ToList();
        }

        private void MakeGroup(Group a)
        {
            var q = Csud.Subject.Where(x => x.SubjectType != Const.Subject.Group);
            a.RelatedKeys = Take(q, 3).Select(a => a.Key).ToList();
            a.Context = Take(Csud.Context, 1).First();
        }

        public void Generate(Dictionary<Type, int> dict)
        {
            input = dict;
            result = new Dictionary<Type, int>();
            current = new Dictionary<Type, int>();
            foreach (var p in dict)
            {
                result[p.Key] = 1;
                current[p.Key] =0;
            }

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

            while (!Ready<Subject>())
                Make<Subject>(MakeSubject);

            while (!Ready<Group>())
                MakeRelative<Group, Subject>(MakeGroup);

            while (!Ready<Account>())
                Make<Account>(MakeAccount);

            while (!Ready<ObjectX>())
                Make<ObjectX>(MakeObject);

            while (!Ready<TaskX>())
                MakeRelative<TaskX, ObjectX>(MakeTask);
        }
    }
}
