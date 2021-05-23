using System;
using System.Collections;
using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Models.Rules;

namespace Csud.Crud.Services
{
    public interface IContextService
    {
        public void Delete(int key);
        public object Copy(int key);
        public object Add(BaseContext entity);
        public IQueryable<T> Select<T>(string status = Const.Status.Actual) where T : Context;
        public object Get(int key, string status = Const.Status.Actual);
        public IEnumerable List(string type, string status = Const.Status.Actual, int skip = 0, int take = 0);
        public T Update<T>(T entity) where T : BaseContext;
        public object Include(int key, int relatedKey);
        public object Exclude(int key, int relatedKey);
    }

    public class ContextService: IContextService
    {
        protected IEntityService<Context> CommonService;
        protected IOneToOneService<TimeContext, TimeContextAdd, TimeContextEdit, Context> TimeService;
        protected IOneToOneService<SegmentContext, SegmentContextAdd, SegmentContextEdit, Context> SegmentService;
        protected IOneToOneService<RuleContext, RuleContextAdd, RuleContextEdit, Context>  RuleService;
        protected IOneToOneService<StructContext, StructContextAdd, StructContextEdit, Context> StructService;
        protected IOneToOneService<AttributeContext, AttributeContextAdd, AttributeContextEdit, Context> AttributeService;
        protected IOneToManyService<CompositeContext, CompositeContextAdd, CompositeContextEdit, Context> CompositeService;

        public ContextService(IEntityService<Context> contextService, 
            IOneToOneService<TimeContext, TimeContextAdd, TimeContextEdit, Context> timeService,
            IOneToOneService<SegmentContext, SegmentContextAdd, SegmentContextEdit, Context> segmentService,
            IOneToOneService<RuleContext, RuleContextAdd, RuleContextEdit, Context> ruleService,
            IOneToOneService<StructContext, StructContextAdd, StructContextEdit, Context> structService,
            IOneToOneService<AttributeContext, AttributeContextAdd, AttributeContextEdit, Context> attributeService,
            IOneToManyService<CompositeContext, CompositeContextAdd, CompositeContextEdit, Context> compositeService
            )
        {
            CommonService = contextService;
            TimeService = timeService;
            SegmentService = segmentService;
            RuleService = ruleService;
            StructService = structService;
            AttributeService = attributeService;
            CompositeService = compositeService;
        }

        public void Delete(int key)
        {
            var co = this.CommonService.Look(key);
            switch (co.ContextType)
            {
                case Const.Context.Time:
                    TimeService.Delete(key);
                    break;
                case Const.Context.Attrib:
                    AttributeService.Delete(key);
                    break;
                case Const.Context.Rule:
                    RuleService.Delete(key);
                    break;
                case Const.Context.Struct:
                    StructService.Delete(key);
                    break;
                case Const.Context.Segment:
                    SegmentService.Delete(key);
                    break;
                case Const.Context.Composite:
                    CompositeService.Delete(key);
                    break;
                default:
                    throw new ArgumentException("Недопустимый код контекста");
            }
            CommonService.Delete(co);
        }

        public object Add(BaseContext entity)
        {
            return entity.ContextType switch
            {
                Const.Context.Time => TimeService.Add((TimeContextAdd)entity),
                Const.Context.Attrib => AttributeService.Add((AttributeContextAdd)entity),
                Const.Context.Rule => RuleService.Add((RuleContextAdd)entity),
                Const.Context.Struct => StructService.Add((StructContextAdd)entity),
                Const.Context.Segment => SegmentService.Add((SegmentContextAdd)entity),
                Const.Context.Composite => CompositeService.Add((CompositeContextAdd)entity),
                _ => throw new NotImplementedException()
            };
        }


        public T Update<T>(T entity) where T : BaseContext
        {
            switch (entity.ContextType)
            {
                case Const.Context.Time:
                    TimeService.Update(entity as TimeContextEdit);
                    break;
                case Const.Context.Attrib:
                    AttributeService.Update(entity as AttributeContextEdit);
                    break;
                case Const.Context.Rule:
                    RuleService.Update(entity as RuleContextEdit);
                    break;
                case Const.Context.Struct:
                    StructService.Update(entity as StructContextEdit);
                    break;
                case Const.Context.Segment:
                    SegmentService.Update(entity as SegmentContextEdit);
                    break;
                case Const.Context.Composite:
                    CompositeService.Update(entity as CompositeContextEdit);
                    break;
            }
            return entity;
        }

        public object Include(int key, int relatedKey)
        {
            return CompositeService.Include(key, relatedKey);
        }

        public object Exclude(int key, int relatedKey)
        {
            return CompositeService.Exclude(key, relatedKey);
        }

        public object Copy(int key)
        {
            var context = this.CommonService.Look(key);
            return context.ContextType switch
            {
                Const.Context.Time => TimeService.Copy(key, true),
                Const.Context.Attrib => AttributeService.Copy(key, true),
                Const.Context.Rule => RuleService.Copy(key, true),
                Const.Context.Struct => StructService.Copy(key, true),
                Const.Context.Segment => SegmentService.Copy(key, true),
                Const.Context.Composite => CompositeService.Copy(key, true),
                _ => throw new ArgumentException("Недопустимый код контекста")
            };
        }

        public IQueryable<T> Select<T>(string status = Const.Status.Actual) where T : Context
        {
            return CommonService.Select(status).OfType<T>();
        }

        public object Get(int key, string status = Const.Status.Actual)
        {
            var co = CommonService.Select(status).First(a => a.Key == key);
            switch (co.ContextType)
            {
                case Const.Context.Time:
                    return TimeService.Get(key);
                case Const.Context.Attrib:
                    return AttributeService.Get(key);
                case Const.Context.Rule:
                    return RuleService.Get(key);
                case Const.Context.Struct:
                    return StructService.Get(key);
                case Const.Context.Segment:
                    return SegmentService.Get(key);
                case Const.Context.Composite:
                    var c = CompositeService.Get(key, status);
                    return c;
                default:
                    throw new ArgumentException("Недопустимый код контекста");
            }
        }
        public IEnumerable List(string type, string status = Const.Status.Actual, int skip = 0, int take = 0)
        {
            if (!string.IsNullOrEmpty(type) && !Const.Context.Has(type))
                throw new ArgumentException("Недопустимый код контекста");

            var q = CommonService.Select(status);
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
    }
}
