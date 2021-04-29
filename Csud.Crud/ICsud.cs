using System;
using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;

namespace Csud.Crud
{
    public interface ICsud
    {
        void AddEntity<T>(T entity) where T : Base;

        IQueryable<T> Q<T>() where T : Base;

        void AddContext<T>(T entity, bool temp = false) where T : BaseContext
        {
            var context = new Context();
            entity.CopyTo(context);
            context.Temporary = temp;
            AddEntity(context);
            entity.ContextKey = context.Key;
            AddEntity(entity);
        }

        IQueryable<Person> Person => Q<Person>();

        IQueryable<AccountProvider> AccountProvider => Q<AccountProvider>();

        IQueryable<Account> Account => Q<Account>();

        IQueryable<Subject> Subject => Q<Subject>();

        IQueryable<ObjectX> Object => Q<ObjectX>();

        IQueryable<Context> Context => Q<Context>();

        IQueryable<TimeContext> TimeContext => Q<TimeContext>();

        IQueryable<SegmentContext> SegmentContext => Q<SegmentContext>();

        IQueryable<RuleContext> RuleContext => Q<RuleContext>();

        IQueryable<StructContext> StructContext => Q<StructContext>();

        IQueryable<CompositeContext> CompositeContext => Q<CompositeContext>();

        IQueryable<AttribContext> AttribContext =>  
            throw new NotImplementedException($"{typeof(AttribContext)} is not implemented");

        IQueryable<TaskX> Task => Q<TaskX>();

        void AddPerson(Person person) => AddEntity(person);
        void AddAccountProvider(AccountProvider provider) => AddEntity(provider);
        void AddAccount(Account account) => AddEntity(account);
        void AddSubject(Subject subject) => AddEntity(subject);
        void AddObject(ObjectX objectX) => AddEntity(objectX);
        void AddTimeContext(TimeContext timeContext, bool temp = false) => AddContext(timeContext, temp);
        void AddSegmentContext(SegmentContext segmentContext, bool temp = false) => AddContext(segmentContext, temp);
        void AddStructContext(StructContext structContext, bool temp = false) => AddContext(structContext, temp);
        void AddRuleContext(RuleContext ruleContext, bool temp = false) => AddContext(ruleContext, temp);
        void AddCompositeContext(CompositeContext compositeContext, bool temp = false) => AddContext(compositeContext, temp);
        void AddAttribContext(AttribContext attribContext, bool temp = false) => AddContext(attribContext, temp);
    }
}
