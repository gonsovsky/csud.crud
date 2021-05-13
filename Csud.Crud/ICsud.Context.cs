using System;
using System.Collections;
using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Models.Rules;

namespace Csud.Crud
{
    public partial interface ICsud
    {
        public void DeleteContext(int key)
        {
            var co = this.Context.First(a => a.Key == key);
            switch (co.ContextType)
            {
                case Const.Context.Time:
                    DeleteEntity(TimeContext.First(x => x.Key == key));
                    break;
                case Const.Context.Attrib:
                    DeleteEntity(AttributeContext.First(x => x.Key == key));
                    break;
                case Const.Context.Rule:
                    DeleteEntity(RuleContext.First(x => x.Key == key));
                    break;
                case Const.Context.Struct:
                    DeleteEntity(StructContext.First(x => x.Key == key));
                    break;
                case Const.Context.Segment:
                    DeleteEntity(SegmentContext.First(x => x.Key == key));
                    break;
                case Const.Context.Composite:
                    var compositeAll = CompositeContext.Where(x => x.Key == key);
                    foreach (var composite in compositeAll)
                    {
                        DeleteEntity(composite);
                    }
                    break;
                default:
                    throw new ArgumentException("Недопустимый код контекста");
            }
            DeleteEntity(co);
        }
        public void CopyContext(int key)
        {
            var co = this.Context.First(a => a.Key == key);
            co = CopyEntity(co);
            switch (co.ContextType)
            {
                case Const.Context.Time:
                    var time = TimeContext.First(x => x.Key == key);
                    time.Key = co.Key;
                    CopyEntity(time, true);
                    break;
                case Const.Context.Attrib:
                    var attrib = AttributeContext.First(x => x.Key == key);
                    attrib.Key = co.Key;
                    CopyEntity(attrib, true);
                    break;
                case Const.Context.Rule:
                    var rule = RuleContext.First(x => x.Key == key);
                    rule.Key = co.Key;
                    CopyEntity(rule, true);
                    break;
                case Const.Context.Struct:
                    var structX = StructContext.First(x => x.Key == key);
                    structX.Key = co.Key;
                    CopyEntity(structX, true);
                    break;
                case Const.Context.Segment:
                    var segment = SegmentContext.First(x => x.Key == key);
                    segment.Key = co.Key;
                    CopyEntity(segment, true);
                    break;
                case Const.Context.Composite:
                    var compositeAll = CompositeContext.Where(x => x.Key == key);
                    foreach (var composite in compositeAll)
                    {
                        composite.Key = co.Key;
                        CopyEntity(composite, true);
                    }
                    break;
                default:
                    throw new ArgumentException("Недопустимый код контекста");
            }
        }
        public void AddContext<T>(T entity, bool isTemporary = false) where T : BaseContext
        {
            if (entity is CompositeContext compositeContext)
            {
                if (compositeContext.RelatedKeys == null || compositeContext.RelatedKeys.Count == 0)
                    throw new ArgumentException($"Связанные контексты не найдены");
                foreach (var rkey in compositeContext.RelatedKeys)
                {
                    if (Context.Any(a => a.Key == rkey) == false)
                        throw new ArgumentException($"Контекст с кодом {rkey} не найден");
                }
            }
            var context = new Context();
            entity.CopyTo(context, false);
            context.Temporary = isTemporary;
            AddEntity(context);
            entity.Key = context.Key;
            entity.ID = context.ID;
            if (entity is CompositeContext composite)
            {
                foreach (var rkey in composite.RelatedKeys)
                {
                    var x = (CompositeContext)composite.Clone();
                    x.ID = null;
                    x.Key = context.Key;
                    x.RelatedKey = rkey;
                    AddEntity(x, false);
                }
            }
            else
            {
                entity.Key = context.Key;
                AddEntity(entity, false);
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
                    return this.Select<AttributeContext>(status).First(x => x.Key == key);
                case Const.Context.Rule:
                    return this.Select<RuleContext>(status).First(x => x.Key == key);
                case Const.Context.Struct:
                    return this.Select<StructContext>(status).First(x => x.Key == key);
                case Const.Context.Segment:
                    return this.Select<SegmentContext>(status).First(x => x.Key == key);
                case Const.Context.Composite:
                    var c = this.Select<CompositeContext>(status).First(x => x.Key == key);
                    c.RelatedEntities = ExpandCompositeContext(key);
                    return c;
                default:
                    throw new ArgumentException("Недопустимый код контекста");
            }
        }
        public IEnumerable ListContext(string type, string status = Const.Status.Actual, int skip = 0, int take = 0)
        {
            if (!string.IsNullOrEmpty(type) && !Const.Context.Has(type))
                throw new ArgumentException("Недопустимый код контекста");

            var q = Select<Context>(status);
            if (!string.IsNullOrEmpty(type))
                q = q.Where(a => a.ContextType == type);
            if (skip != 0)
                q = q.Skip(skip);
            if (take != 0)
                q = q.Take(take);
            foreach (var context in q)
            {
                yield return GetContext(context.Key, status);
            }
        }
        private IEnumerable ExpandCompositeContext(int key, string status = Const.Status.Actual)
        {
            var all = Select<CompositeContext>(status)
                .Where(a => a.Key == key);
            foreach (var context in all)
                yield return GetContext(context.RelatedKey, status);
        }
        public IQueryable<Context> Context => Select<Context>();
        public IQueryable<TimeContext> TimeContext => Select<TimeContext>();
        public IQueryable<SegmentContext> SegmentContext => Select<SegmentContext>();
        public IQueryable<RuleContext> RuleContext => Select<RuleContext>();
        public IQueryable<StructContext> StructContext => Select<StructContext>();
        public IQueryable<CompositeContext> CompositeContext => Select<CompositeContext>();
        public IQueryable<AttributeContext> AttributeContext =>
            throw new NotImplementedException($"{typeof(AttributeContext)} is not implemented");
    }
}
