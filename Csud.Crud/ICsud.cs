using System;
using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;

namespace Csud.Crud
{
    public interface ICsud
    {
        public void AddEntity<T>(T entity, bool idPredefined = false) where T : Base;

        public void UpdateEntity<T>(T entity) where T : Base;

        public void DelEntity<T>(T entity) where T : Base
        {
            entity.Status = Const.StatusRemoved;
            UpdateEntity(entity);
        }

        public void CopyEntity<T>(T entity) where T : Base
        {
            var a = (T)entity.Clone();
            AddEntity(a);
        }

        public void RestoreEntity<T>(T entity) where T : Base
        {
            entity.Status = Const.StatusActual;
            UpdateEntity(entity);
        }

        public IQueryable<T> Q<T>() where T : Base;

        public void AddContext<T>(T entity, bool isTemporary = false) where T : BaseContext
        {
            var context = new Context();
            entity.CopyTo(context,false);
            context.Temporary = isTemporary;
            AddEntity(context);
            entity.Key = context.Key;
            AddEntity(entity,true);
        }

        public IQueryable<Person> Person => Q<Person>();

        public IQueryable<AccountProvider> AccountProvider => Q<AccountProvider>();

        public IQueryable<Account> Account => Q<Account>();

        public IQueryable<Subject> Subject => Q<Subject>();

        public IQueryable<ObjectX> Object => Q<ObjectX>();

        public IQueryable<Context> Context => Q<Context>();

        public IQueryable<TimeContext> TimeContext => Q<TimeContext>();

        public IQueryable<SegmentContext> SegmentContext => Q<SegmentContext>();

        public IQueryable<RuleContext> RuleContext => Q<RuleContext>();

        public IQueryable<StructContext> StructContext => Q<StructContext>();

        public IQueryable<CompositeContext> CompositeContext => Q<CompositeContext>();

        public IQueryable<AttribContext> AttribContext =>  
            throw new NotImplementedException($"{typeof(AttribContext)} is not implemented");

        public IQueryable<TaskX> Task => Q<TaskX>();

        public void AddPerson(Person person) => AddEntity(person);
        public void AddAccountProvider(AccountProvider provider) => AddEntity(provider);
        public void AddAccount(Account account) => AddEntity(account);
        public void AddSubject(Subject subject) => AddEntity(subject);
        public void AddObject(ObjectX objectX) => AddEntity(objectX);
        public void AddTimeContext(TimeContext timeContext, bool temp = false) => AddContext(timeContext, temp);
        public void AddSegmentContext(SegmentContext segmentContext, bool temp = false) => AddContext(segmentContext, temp);
        public void AddStructContext(StructContext structContext, bool temp = false) => AddContext(structContext, temp);
        public void AddRuleContext(RuleContext ruleContext, bool temp = false) => AddContext(ruleContext, temp);
        public void AddCompositeContext(CompositeContext compositeContext, bool temp = false) => AddContext(compositeContext, temp);
        public void AddAttribContext(AttribContext attribContext, bool temp = false) => AddContext(attribContext, temp);
    }
}
