using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Reflection;
using Csud.Crud.Models;
using Csud.Crud.Models.App;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Models.Maintenance;
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
                props.sourceProperty.SetValue(source, $"{props.sourceProperty.Name} - {No} / {From<T>()}");
            }
            source.Status = Const.Status.Actual;
            return source;
        }

        private Dictionary<Type, int> current;

        private IEnumerable<T> Take<T>(IQueryable<T> q, int number) where T : Base
        {
            var z = current[typeof(T)];
            var r = q.Skip(z).Take(number).ToList();
            var no = r.Count();
            var cnt = q.Count();
            current[typeof(T)] += no;
            if (current[typeof(T)] >= cnt)
                current[typeof(T)] = 0;
        
            return r;
        }

        private void Log<T>() where T : Base
        {
            var s = $@"{typeof(T).Name} - {From<T>()}/{To<T>()}";
            Console.WriteLine(s);
            result[typeof(T)] += 1;
            No += 1;
        }

        private void Make<T>(Action<T> act = null, bool generateKey=true) where T : Base
        {
            var a = Gen<T>();
            if (act != null) act(a);
            Csud.AddEntity(a, generateKey);
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

        #region App
        private void AppMakeApp(App a)
        {

        }

        private void AppMakeAppDistrib(AppDistrib a)
        {
            a.AppKey = Take(Csud.App, 1).Select(a => a.AppKey).First();
            a.LoadDate = No;
        }

        private void AppMakeRoleApp(AppRole a)
        {
            a.DistribKey = Take(Csud.AppDistrib, 1).First().DistribKey;
        }

        private void AppMakeRoleAppDefinition(AppRoleDefinition a)
        {
            a.ObjectKey = Take(Csud.Select<ObjectX>(), 1).First().Key;
        }

        private void AppMakeRoleAppRoleDetails(AppRoleDetails a)
        {
            a.RoleKey = Take(Csud.Select<AppRoleDefinition>(), 1).First().RoleKey;
            a.OperationKey = Take(Csud.Select<AppOperationDefinition>(), 1).First().OperationKey;
        }


        private void AppMakeEntityDefinition(AppEntityDefinition a)
        {
            a.ObjectKey = Take(Csud.Select<ObjectX>(), 1).First().Key;
        }

        private void AppMakeEntity(AppEntity a)
        {
            a.DistribKey = Take(Csud.AppDistrib, 1).First().DistribKey;
            a.EntityKey = Take(Csud.AppEntityDefinition, 1).First().EntityKey;
        }

        private void AppMakeAttributeDefinition(AppAttributeDefinition a)
        {
            a.ObjectKey = Take(Csud.Select<ObjectX>(), 1).First().Key;
        }

        private void AppMakeAttribute(AppAttribute a)
        {
            a.DistribKey = Take(Csud.AppDistrib, 1).First().DistribKey;
            a.AttributeKey = Take(Csud.AppAttributeDefinition, 1).First().AttributeKey;
        }

        private void AppMakeOperation(AppOperation a)
        {
            a.DistribKey = Take(Csud.AppDistrib, 1).First().DistribKey;
            a.OperationKey = Take(Csud.AppOperationDefinition, 1).First().OperationKey;
        }

        private void AppMakeOperationDefinition(AppOperationDefinition a)
        {
            a.ObjectKey = Take(Csud.Select<ObjectX>(), 1).First().Key;
        }
        #endregion



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

            #region App
            if (CsudService.Config.IsDebugMode || 1 == 1)
            {
                while (!Ready<App>())
                    Make<App>(AppMakeApp);

                while (!Ready<AppDistrib>())
                    Make<AppDistrib>(AppMakeAppDistrib);
                foreach (var a in Csud.App)
                {
                    a.LastDistribKey = Take(Csud.AppDistrib.OrderBy(b => b.LoadDate), 1).First().DistribKey;
                    Csud.UpdateEntity(a);
                }

                while (!Ready<AppRole>())
                    Make<AppRole>(AppMakeRoleApp, false);

                while (!Ready<AppRoleDefinition>())
                    Make<AppRoleDefinition>(AppMakeRoleAppDefinition);
                foreach (var a in Csud.AppRole)
                {
                    a.RoleKey = Take(Csud.AppRoleDefinition, 1).First().RoleKey;
                    Csud.UpdateEntity(a);
                }

                while (!Ready<AppEntityDefinition>())
                    Make<AppEntityDefinition>(AppMakeEntityDefinition);

                while (!Ready<AppEntity>())
                    Make<AppEntity>(AppMakeEntity, false);

                while (!Ready<AppAttributeDefinition>())
                    Make<AppAttributeDefinition>(AppMakeAttributeDefinition);

                while (!Ready<AppAttribute>())
                    Make<AppAttribute>(AppMakeAttribute, false);

                while (!Ready<AppOperationDefinition>())
                    Make<AppOperationDefinition>(AppMakeOperationDefinition);

                while (!Ready<AppOperation>())
                    Make<AppOperation>(AppMakeOperation, false);

                while (!Ready<AppRoleDetails>())
                    Make<AppRoleDetails>(AppMakeRoleAppRoleDetails, false);

                while (!Ready<AppImport>())
                    Make<AppImport>();
            }
            #endregion
        }
    }
}
