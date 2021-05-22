using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Csud.Crud.Models;
using Csud.Crud.Models.App;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Models.Maintenance;
using Csud.Crud.Models.Rules;
using Csud.Crud.Services;
using Csud.Crud.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Csud.Crud.DbTool
{
    public interface IGeneratorService
    {
        public void Run(Dictionary<Type, int> dict);
    };

    public class GeneratorService: IGeneratorService
    {
        protected int No;

        protected IDbService Svc;

        protected IContextService ContextSvc;

        protected Config Config;

        public GeneratorService(IDbService svc,IContextService svcContext, Config config)
        {
            Config = config;
            Svc = svc;
            ContextSvc = svcContext;
        }

        public void Run(Dictionary<Type, int> dict)
        {
            input = dict;
            result = new Dictionary<Type, int>();
            current = new Dictionary<Type, int>();
            foreach (var p in dict)
            {
                result[p.Key] = 1;
                current[p.Key] = 0;
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
                MakeContext<CompositeContextAdd>(MakeCompositeContext);

            while (!Ready<Subject>())
                Make<Subject>(MakeSubject);

            while (!Ready<Group>())
                MakeOneToMany<Group, GroupAdd, GroupEdit, Subject>(MakeGroup);

            while (!Ready<Account>())
                Make<Account>(MakeAccount);

            while (!Ready<ObjectX>())
                Make<ObjectX>(MakeObject);

            while (!Ready<TaskX>())
                MakeOneToMany<TaskX, TaskAdd, TaskEdit, ObjectX>(MakeTask);


            while (!Ready<App>())
                Make<App>(AppMakeApp);

            while (!Ready<AppDistrib>())
                Make<AppDistrib>(AppMakeAppDistrib);
            foreach (var a in Svc.App)
            {
                a.LastDistribKey = Take(Svc.AppDistrib.OrderBy(b => b.LoadDate), 1).First().DistribKey;
                Svc.Update(a);
            }

            while (!Ready<AppRole>())
                Make<AppRole>(AppMakeRoleApp, false);

            while (!Ready<AppRoleDefinition>())
                Make<AppRoleDefinition>(AppMakeRoleAppDefinition);
            foreach (var a in Svc.AppRole)
            {
                a.RoleKey = Take(Svc.AppRoleDefinition, 1).First().RoleKey;
                Svc.Update(a);
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
            Svc.Add(a, generateKey);
            Log<T>();
        }

        private void MakeContext<T>(Action<T> act = null) where T : BaseContext
        {
            var a = Gen<T>();
            act?.Invoke(a);
            ContextSvc.Add(a);
            Log<T>();
        }

        private void MakeOneToMany<TEntity,TModelAdd, TModelEdit, TLinked>(Action<TModelAdd> act = null) 
            where TEntity : Base, IOneToMany 
            where TModelAdd: TEntity, IOneToManyAdd 
            where TModelEdit: TEntity, IOneToManyEdit 
            where TLinked : Base
        {
            var svc = Program.scope.ServiceProvider.GetRequiredService<IOneToManyService<TEntity, TModelAdd, TModelEdit, TLinked >>();
            var a = Gen<TModelAdd>();
            var b = Gen<TLinked>();
            act?.Invoke(a);
            svc.Add(a,true);
            Log<TEntity>();
        }

        private void MakeTimeContext(TimeContext a)
        {
            a.TimeStart = 1000000 + No * 1000000;
            a.TimeEnd = 1000000 + (No+1) * 1000000;
        }

        private void MakeCompositeContext(CompositeContextAdd a)
        {
            var q = Svc.Context.Where(x => x.ContextType != Const.Context.Composite);
            a.RelatedKeys = Take(q, 3).Select(a => a.Key).ToList();
        }

        private void MakeSubject(Subject a)
        {
            a.ContextKey = Take(Svc.Context, 1).First().Key;
            a.SubjectType = Const.Subject.Account;
        }

        private void MakeAccount(Account a)
        {
            a.AccountProviderKey = Take(Svc.AccountProvider, 1).First().Key;
            a.SubjectKey = Take(Svc.Subject, 1).First().Key;
            a.Person = Take(Svc.Person, 1).First();
        }
        private void MakeObject(ObjectX a)
        {
            a.Context = Take(Svc.Context, 1).First();
            a.ObjectType = Const.Object.Entity;
        }

        private void MakeTask(TaskAdd a)
        {
            var q = Svc.Object.Where(x => x.ObjectType != Const.Object.Task);
            a.RelatedKeys = Take(q, 3).Select(a => a.Key).ToList();
        }

        private void MakeGroup(GroupAdd a)
        {
            var q = Svc.Subject.Where(x => x.SubjectType != Const.Subject.Group);
            a.RelatedKeys = Take(q, 3).Select(a => a.Key).ToList();
            a.Context = Take(Svc.Context, 1).First();
        }

        #region App
        private void AppMakeApp(App a)
        {

        }

        private void AppMakeAppDistrib(AppDistrib a)
        {
            a.AppKey = Take(Svc.App, 1).Select(a => a.AppKey).First();
            a.LoadDate = No;
        }

        private void AppMakeRoleApp(AppRole a)
        {
            a.DistribKey = Take(Svc.AppDistrib, 1).First().DistribKey;
        }

        private void AppMakeRoleAppDefinition(AppRoleDefinition a)
        {
            a.ObjectKey = Take(Svc.Select<ObjectX>(), 1).First().Key;
        }

        private void AppMakeRoleAppRoleDetails(AppRoleDetails a)
        {
            a.RoleKey = Take(Svc.Select<AppRoleDefinition>(), 1).First().RoleKey;
            a.OperationKey = Take(Svc.Select<AppOperationDefinition>(), 1).First().OperationKey;
        }


        private void AppMakeEntityDefinition(AppEntityDefinition a)
        {
            a.ObjectKey = Take(Svc.Select<ObjectX>(), 1).First().Key;
        }

        private void AppMakeEntity(AppEntity a)
        {
            a.DistribKey = Take(Svc.AppDistrib, 1).First().DistribKey;
            a.EntityKey = Take(Svc.AppEntityDefinition, 1).First().EntityKey;
        }

        private void AppMakeAttributeDefinition(AppAttributeDefinition a)
        {
            a.ObjectKey = Take(Svc.Select<ObjectX>(), 1).First().Key;
        }

        private void AppMakeAttribute(AppAttribute a)
        {
            a.DistribKey = Take(Svc.AppDistrib, 1).First().DistribKey;
            a.AttributeKey = Take(Svc.AppAttributeDefinition, 1).First().AttributeKey;
        }

        private void AppMakeOperation(AppOperation a)
        {
            a.DistribKey = Take(Svc.AppDistrib, 1).First().DistribKey;
            a.OperationKey = Take(Svc.AppOperationDefinition, 1).First().OperationKey;
        }

        private void AppMakeOperationDefinition(AppOperationDefinition a)
        {
            a.ObjectKey = Take(Svc.Select<ObjectX>(), 1).First().Key;
        }
        #endregion
    }
}
