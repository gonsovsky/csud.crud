using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using Csud.Crud.Models.Rules;
using Csud.Crud.Services;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Csud.Crud.Models.Contexts
{

    public class CompositeContext : BaseContext, IOneToMany
    {
        public virtual int RelatedKey { get; set; }
        [NotMapped] [BsonIgnore] [Ignore] [JsonIgnore] public virtual IEnumerable RelatedEntities { get; set; }

        public void Link(Base linked)
        {
            ((Context) linked).ContextType = ContextType;
        }
        public IOneToManyItem<TEntity, TLinked> MakeOneToManyItem<TEntity, TLinked>(TEntity relation, TLinked related)
            where TEntity : Base, IOneToMany
            where TLinked : Base
        {
            return new ContextOneToManyItem<TEntity, TLinked>()
            {
                Relation = relation,
                Related = related
            };
        }

        public IOneToManyRecord<TEntity, TLinked> MakeOneToManyRecord<TEntity, TLinked>(TLinked relation,
            IEnumerable<IOneToManyItem<TEntity, TLinked>> relations)
            where TEntity : Base, IOneToMany
            where TLinked : Base
        {
            return new ContextOneToManyRecord<TEntity, TLinked>()
            {
                Relation = relation,
                Relations = relations
            };
        }

        public override string ContextType => Const.Context.Composite;
    }


    public class CompositeContextEdit : CompositeContext, INoneRepo, IOneToManyEdit
    {
        [JsonIgnore] public override int Key { get; set; }
        [JsonIgnore] public override int RelatedKey { get; set; }

        [JsonIgnore] public virtual List<int> RelatedKeys { get; set; }
    }

    [CompositeContextEditValidation]
    public class CompositeContextAdd : CompositeContext, IOneToManyAdd
    {
        public List<int> RelatedKeys { get; set; }
    }

    internal class CompositeContextEditValidationAttribute : BaseValidator
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Reset();
            if (!(value is CompositeContextAdd))
                return null;

            var entity = (CompositeContextAdd)value;
            var service = (IEntityService<Context>)validationContext
                .GetService(typeof(IEntityService<Context>));

            if (entity.RelatedKeys == null || entity.RelatedKeys.Count == 0)
                return new ValidationResult($"Связанные контексты не найдены");
            foreach (var rkey in ((entity.RelatedKeys) ?? throw new InvalidOperationException())
                .Where(rkey => service != null && service.Select().Any(a => a.Key == rkey) == false))
            {
                return new ValidationResult($"Связанный контекст с ключем {rkey} не найден");
            }
            return null;
        }
    }

    public class ContextOneToManyItem<TEntity, TLinked> : IOneToManyItem<TEntity, TLinked>
        where TEntity : Base, IOneToMany
        where TLinked : Base
    {
        public TEntity Relation { get; set; }

        public TLinked Related { get; set; }
    }

    public class ContextOneToManyRecord<TEntity, TLinked> : IOneToManyRecord<TEntity, TLinked>
        where TEntity : Base, IOneToMany
        where TLinked : Base
    {
        [JsonPropertyName("context")]
        public TLinked Relation { get; set; }

        public IEnumerable<IOneToManyItem<TEntity, TLinked>> Relations { get; set; }
            = new List<GroupOneToManyItem<TEntity, TLinked>>() { };
    }
}
