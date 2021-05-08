using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Models.Rules;

namespace Csud.Crud
{
    public interface ICsud
    {
        public void AddEntity<T>(T entity, bool keyDefined = false) where T : Base;

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
            entity.CopyTo(context, false);
            context.Temporary = isTemporary;
            AddEntity(context);
            if (entity is CompositeContext)
            {
                var composite = entity as CompositeContext;
                var all = composite.Decompose();
                foreach (var co in all)
                {
                    var x = (CompositeContext)entity.Clone();
                    x.Key = context.Key;
                    x.RelatedKey = co.RelatedKey;
                    AddEntity(x, true);
                }
            }
            else
            {
                entity.Key = context.Key;
                AddEntity(entity, true);
            }
        }

        public Context GetContext(int? key)
        {
            var co = this.Context.First(x => x.Key == key);
            switch (co.ContextType)
            {
                case "time":
                    co.Details = this.TimeContext.First(x => x.Key == key);
                    break;
                case "attrib":
                    co.Details = this.AttribContext.First(x => x.Key == key);
                    break;
                case "rule":
                    co.Details = this.RuleContext.First(x => x.Key == key);
                    break;
                case "struct":
                    co.Details = this.StructContext.First(x => x.Key == key);
                    break;
                case "segment":
                    co.Details = this.SegmentContext.First(x => x.Key == key);
                    break;
                case "composite":
                    co.Details = this.CompositeContext.First(x => x.Key == key);
                    break;
                default:
                    throw new ArgumentException("Недопустимый код операции");
            }
            return co;
        }

        public IEnumerable ListContext(int skip=0, int take=0)
        {
            var q = Context;
            if (skip != 0)
                q = q.Skip(skip);
            if (take != 0)
                q = q.Take(take);
            foreach (var context in q)
            {
                yield return GetContext(context.Key);
            }
        }

        public IQueryable<T> List<T>(int skip = 0, int take = 0) where T: Base
        {
            var q = Q<T>();
            if (skip != 0)
                q = q.Skip(skip);
            if (take != 0)
                q = q.Take(take);
            return q;
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
        public void AddTimeContext(TimeContext timeContext, bool isTemporary = false) => AddContext(timeContext, isTemporary);
        public void AddSegmentContext(SegmentContext segmentContext, bool isTemporary = false) => AddContext(segmentContext, isTemporary);
        public void AddStructContext(StructContext structContext, bool isTemporary = false) => AddContext(structContext, isTemporary);
        public void AddRuleContext(RuleContext ruleContext, bool isTemporary = false) => AddContext(ruleContext, isTemporary); 
        public void AddAttribContext(AttribContext attribContext, bool isTemporary = false) => AddContext(attribContext, isTemporary);
        public void AddCompositeContext(CompositeContext compositeContext, bool isTemporary = false) => AddContext(compositeContext, isTemporary);
    }
}
