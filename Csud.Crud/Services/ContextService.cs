using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Models.Rules;

namespace Csud.Crud.Services
{
    public interface IContextService
    {
        public void Delete(int key);

        public void Copy(int key);

        public void Add<T>(T entity, bool isTemporary = false) where T : BaseContext;
        public BaseContext Get(int key, string status = Const.Status.Actual);

        public IEnumerable List(string type, string status = Const.Status.Actual, int skip = 0, int take = 0);

        public IEnumerable Expand(int key, string status = Const.Status.Actual);
    }

    public class ContextService : IContextService
    {
        protected IEntityService<Context> Context;
        protected IEntityService<TimeContext> TimeContext;
        protected IEntityService<SegmentContext> SegmentContext;
        protected IEntityService<RuleContext> RuleContext;
        protected IEntityService<StructContext> StructContext;
        protected IEntityService<CompositeContext> CompositeContext;
        protected IEntityService<AttributeContext> AttributeContext;

        protected ContextService(IEntityService<Context> context, IEntityService<TimeContext> timeContext,
            IEntityService<SegmentContext> segmentContext, IEntityService<RuleContext> ruleContext,
            IEntityService<StructContext> structContext,
            IEntityService<CompositeContext> compositeContext,
            IEntityService<AttributeContext> attributeContext
            )
        {
            Context= context;
            TimeContext = timeContext;
            SegmentContext = segmentContext;
            RuleContext = ruleContext;
            StructContext = structContext;
            CompositeContext = compositeContext;
            AttributeContext = attributeContext;
        }

        public void Delete(int key)
        {
            var co = this.Context.Look(key);
            switch (co.ContextType)
            {
                case Const.Context.Time:
                    TimeContext.Delete(key);
                    break;
                case Const.Context.Attrib:
                    AttributeContext.Delete(key);
                    break;
                case Const.Context.Rule:
                    RuleContext.Delete(key);
                    break;
                case Const.Context.Struct:
                    StructContext.Delete(key);
                    break;
                case Const.Context.Segment:
                    SegmentContext.Delete(key);
                    break;
                case Const.Context.Composite:
                    CompositeContext.Delete(key);
                    var compositeAll = CompositeContext.Select().Where(x => x.Key == key);
                    foreach (var composite in compositeAll)
                    {
                        CompositeContext.Delete(composite);
                    }
                    break;
                default:
                    throw new ArgumentException("Недопустимый код контекста");
            }
            Context.Delete(co);
        }
        public void Copy(int key)
        {
            var co = this.Context.Select().First(a => a.Key == key);
            co = Context.Copy(co);
            switch (co.ContextType)
            {
                case Const.Context.Time:
                    var time = TimeContext.Select().First(x => x.Key == key);
                    time.Key = co.Key;
                    TimeContext.Copy(time, true);
                    break;
                case Const.Context.Attrib:
                    var attrib = AttributeContext.Select().First(x => x.Key == key);
                    attrib.Key = co.Key;
                    AttributeContext.Copy(attrib, true);
                    break;
                case Const.Context.Rule:
                    var rule = RuleContext.Select().First(x => x.Key == key);
                    rule.Key = co.Key;
                    RuleContext.Copy(rule, true);
                    break;
                case Const.Context.Struct:
                    var structX = StructContext.Select().First(x => x.Key == key);
                    structX.Key = co.Key;
                    StructContext.Copy(structX, true);
                    break;
                case Const.Context.Segment:
                    var segment = SegmentContext.Select().First(x => x.Key == key);
                    segment.Key = co.Key;
                    SegmentContext.Copy(segment, true);
                    break;
                case Const.Context.Composite:
                    var compositeAll = CompositeContext.Select().Where(x => x.Key == key);
                    foreach (var composite in compositeAll)
                    {
                        composite.Key = co.Key;
                        CompositeContext.Copy(composite,false);
                    }
                    break;
                default:
                    throw new ArgumentException("Недопустимый код контекста");
            }
        }

        public void Add<T>(T entity, bool isTemporary = false) where T : BaseContext
        {
            if (entity is CompositeContext compositeContext)
            {
                if (compositeContext.RelatedKeys == null || compositeContext.RelatedKeys.Count == 0)
                    throw new ArgumentException($"Связанные контексты не найдены");
                foreach (var rkey in compositeContext.RelatedKeys)
                {
                    if (Context.Select().Any(a => a.Key == rkey) == false)
                        throw new ArgumentException($"Контекст с кодом {rkey} не найден");
                }
            }

            var context = new Context();
            entity.CopyTo(context, false);
            context.Temporary = isTemporary;
            Context.Add(context);
            entity.Key = context.Key;
            entity.ID = context.ID;
            if (entity is CompositeContext composite)
            {
                foreach (var rkey in composite.RelatedKeys)
                {
                    var x = (CompositeContext) composite.Clone();
                    x.ID = null;
                    x.Key = context.Key;
                    x.RelatedKey = rkey;
                    CompositeContext.Add(x, false);
                }
            }
            else
            {
                entity.Key = context.Key;
                switch (entity.ContextType)
                {
                    case Const.Context.Time:
                        var time = entity.CloneTo<TimeContext>(false);
                        TimeContext.Add(time, true);
                        break;
                    case Const.Context.Attrib:
                        var attrib = entity.CloneTo<AttributeContext>(false);
                        AttributeContext.Add(attrib, true);
                        break;
                    case Const.Context.Rule:
                        var rule = entity.CloneTo<RuleContext>(false);
                        RuleContext.Add(rule, true);
                        break;
                    case Const.Context.Struct:
                        var structX = entity.CloneTo<StructContext>(false);
                        StructContext.Add(structX, true);
                        break;
                    case Const.Context.Segment:
                        var segment = entity.CloneTo<SegmentContext>(false);
                        SegmentContext.Add(segment, true);
                        break;
                }
            }
        }

        public BaseContext Get(int key, string status = Const.Status.Actual)
        {
            var co = Context.Select(status).First(a => a.Key == key);
            switch (co.ContextType)
            {
                case Const.Context.Time:
                    return TimeContext.Select(status).First(x => x.Key == key);
                case Const.Context.Attrib:
                    return AttributeContext.Select(status).First(x => x.Key == key);
                case Const.Context.Rule:
                    return RuleContext.Select(status).First(x => x.Key == key);
                case Const.Context.Struct:
                    return StructContext.Select(status).First(x => x.Key == key);
                case Const.Context.Segment:
                    return SegmentContext.Select(status).First(x => x.Key == key);
                case Const.Context.Composite:
                    var c = CompositeContext.Select(status).First(x => x.Key == key);
                    c.RelatedEntities = Expand(key);
                    return c;
                default:
                    throw new ArgumentException("Недопустимый код контекста");
            }
        }
        public IEnumerable List(string type, string status = Const.Status.Actual, int skip = 0, int take = 0)
        {
            if (!string.IsNullOrEmpty(type) && !Const.Context.Has(type))
                throw new ArgumentException("Недопустимый код контекста");

            var q = Context.Select(status);
            if (!string.IsNullOrEmpty(type))
                q = q.Where(a => a.ContextType == type);
            if (skip != 0)
                q = q.Skip(skip);
            if (take != 0)
                q = q.Take(take);
            foreach (var context in q)
            {
                yield return Get(context.Key, status);
            }
        }
        public IEnumerable Expand(int key, string status = Const.Status.Actual)
        {
            var all = CompositeContext.Select(status)
                .Where(a => a.Key == key);
            foreach (var context in all)
                yield return Get(context.RelatedKey, status);
        }
    }
}
