using System;
using System.Collections;
using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Models.Rules;

namespace Csud.Crud
{
    public interface ICsud
    {
        public void Insert<T>(T entity, bool generateKey = true) where T : Base;
        public void Upd<T>(T entity) where T : Base;
        public void Del<T>(T entity) where T : Base
        {
            entity.Status = Const.Status.Removed;
            Upd(entity);
        }
        public void Copy<T>(T entity, bool keepKey=false) where T : Base
        {
            var a = (T) entity.Clone(keepKey);
            Insert(entity, !keepKey);
        }
        public void Restore<T>(T entity) where T : Base
        {
            entity.Status = Const.Status.Actual;
            Upd(entity);
        }
        public IQueryable<T> Select<T>(string status = Const.Status.Actual) where T : Base;
        public IQueryable<T> List<T>(string status = Const.Status.Actual, int skip = 0, int take = 0) where T : Base
        {
            var q = Select<T>(status);
            if (skip != 0)
                q = q.Skip(skip);
            if (take != 0)
                q = q.Take(take);
            return q;
        }
        #region Context
        public void DelContext(int key)
        {
            var co = this.Context.First(a => a.Key == key);
            switch (co.ContextType)
            {
                case Const.Context.Time:
                    Del(TimeContext.First(x => x.Key == key));
                    break;
                case Const.Context.Attrib:
                    Del(AttribContext.First(x => x.Key == key));
                    break;
                case Const.Context.Rule:
                    Del(RuleContext.First(x => x.Key == key));
                    break;
                case Const.Context.Struct:
                    Del(StructContext.First(x => x.Key == key));
                    break;
                case Const.Context.Segment:
                    Del(SegmentContext.First(x => x.Key == key));
                    break;
                case Const.Context.Composite:
                    Del(SegmentContext.First(x => x.Key == key));
                    break;
                default:
                    throw new ArgumentException("Недопустимый код контекста");
            }

            Del(co);
        }
        public void CopyContext(int key)
        {
            var co = this.Context.First(a => a.Key == key);
            Copy(co);
            switch (co.ContextType)
            {
                case Const.Context.Time:
                    var time = TimeContext.First(x => x.Key == key);
                    time.Key = co.Key;
                    Copy(time, true);
                    break;
                case Const.Context.Attrib:
                    var attrib = AttribContext.First(x => x.Key == key);
                    attrib.Key = co.Key;
                    Copy(attrib, true);
                    break;
                case Const.Context.Rule:
                    var rule = RuleContext.First(x => x.Key == key);
                    rule.Key = co.Key;
                    Copy(rule, true);
                    break;
                case Const.Context.Struct:
                    var structX = StructContext.First(x => x.Key == key);
                    structX.Key = co.Key;
                    Copy(structX, true);
                    break;
                case Const.Context.Segment:
                    var segment = SegmentContext.First(x => x.Key == key);
                    segment.Key = co.Key;
                    Copy(segment, true);
                    break;
                case Const.Context.Composite:
                    var composite = SegmentContext.First(x => x.Key == key);
                    composite.Key = co.Key;
                    Copy(composite, true);
                    break;
                default:
                    throw new ArgumentException("Недопустимый код контекста");
            }
        }
        public void AddContext<T>(T entity, bool isTemporary = false) where T : BaseContext
        {
            var context = new Context();
            entity.CopyTo(context, false);
            context.Temporary = isTemporary;
            Insert(context);
            entity.Key = context.Key;
            entity.ID = context.ID;
            if (entity is CompositeContext)
            {
                var composite = entity as CompositeContext;
                var all = composite.Decompose();
                foreach (var co in all)
                {
                    var x = (CompositeContext) entity.Clone();
                    x.ID = null;
                    x.Key = context.Key;
                    x.RelatedKey = co.RelatedKey;
                    Insert(x, false);
                }
            }
            else
            {
                entity.Key = context.Key;
                Insert(entity, false);
            }
        }
        public BaseContext GetContext(int key, string status = Const.Status.Actual)
        {
            var co = this.Select<Context>(status).First(a => a.Key == key);
            switch (co.ContextType)
            {
                case Const.Context.Time:
                    return this.Select<TimeContext>(status).First(x => x.Key == key);
                case Const.Context.Attrib:
                    return this.Select<AttribContext>(status).First(x => x.Key == key);
                case Const.Context.Rule:
                    return this.Select<RuleContext>(status).First(x => x.Key == key);
                case Const.Context.Struct:
                    return this.Select<StructContext>(status).First(x => x.Key == key);
                case Const.Context.Segment:
                    return this.Select<SegmentContext>(status).First(x => x.Key == key);
                case Const.Context.Composite:
                    var c = new CompositeContext();
                    co.CopyTo(c, true);
                    c.RelatedContexts = ListCompositeContext(key);
                    return c;
                default:
                    throw new ArgumentException("Недопустимый код контекста");
            }
        }
        private IEnumerable ListCompositeContext(int key, string status = Const.Status.Actual)
        {
            var all = Select<CompositeContext>(status)
                .Where(a => a.Key == key);
            foreach (var context in all)
            {
                yield return GetContext((int) context.RelatedKey, status);
            }
        }
        public IEnumerable ListContext(string status = Const.Status.Actual, int skip = 0, int take = 0)
        {
            var q = Select<Context>(status);
            if (skip != 0)
                q = q.Skip(skip);
            if (take != 0)
                q = q.Take(take);
            foreach (var context in q)
            {
                yield return GetContext((int) context.Key, status);
            }
        }
        public IQueryable<Context> Context => Select<Context>();
        public IQueryable<TimeContext> TimeContext => Select<TimeContext>();
        public IQueryable<SegmentContext> SegmentContext => Select<SegmentContext>();
        public IQueryable<RuleContext> RuleContext => Select<RuleContext>();
        public IQueryable<StructContext> StructContext => Select<StructContext>();
        public IQueryable<CompositeContext> CompositeContext => Select<CompositeContext>();
        public IQueryable<AttribContext> AttribContext =>
            throw new NotImplementedException($"{typeof(AttribContext)} is not implemented");
        #endregion
        public IQueryable<Person> Person => Select<Person>();
        public IQueryable<AccountProvider> AccountProvider => Select<AccountProvider>();
        public IQueryable<Account> Account => Select<Account>();
        public IQueryable<Subject> Subject => Select<Subject>();
        public IQueryable<ObjectX> Object => Select<ObjectX>();
        public IQueryable<TaskX> Task => Select<TaskX>();
    }
}
