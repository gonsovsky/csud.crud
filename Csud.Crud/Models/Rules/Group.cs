using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using Csud.Crud.Models.Internal;
using Csud.Crud.Services;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Csud.Crud.Models.Rules
{
    internal class GroupValidator : BaseValidator
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Reset();
            var service = (IEntityService<Subject>)validationContext.GetService(typeof(IEntityService<Subject>));
            if (service == null)
                throw new ApplicationException($"{nameof(EntityService<Subject>)} is not found");

            var serviceCtx = (IContextService)validationContext.GetService(typeof(IContextService));
            if (serviceCtx == null)
                throw new ApplicationException($"{nameof(IContextService)} is not found");

            if (!serviceCtx.Select<Context>().Any(x => x.Key == ((IContextable)value).ContextKey))
            {
                return new ValidationResult("Неверный код контекста.");
            }

            if (value is IOneToManyAdd groupAdd)
            {
                if (!groupAdd.RelatedKeys.Any())
                    return new ValidationResult($"Не указаны связанные объекты");
                foreach (var rkey in groupAdd.RelatedKeys
                    .Where(rkey => !service.Select().Any(a => a.Key == rkey)))
                {
                    return new ValidationResult($"Связанный объект с кодом {rkey} не найден");
                }
            }
            else if (value is IOneToManyEdit groupEdit)
            {
                if (!service.Select().Any(x => x.Key == groupEdit.RelatedKey))
                {
                    return new ValidationResult($"Неверный код {groupEdit.RelatedKey} связанного объекта.");
                }
            }
            return null;
        }
    }

    [GroupValidator]
    public class Group: Base, IOneToMany, IContextable, IWellNamed
    {
        protected override string QueueName => "Group";
        [NotMapped] [Ignore] [BsonIgnore] public string Name { get; set; }
        [NotMapped] [Ignore] [BsonIgnore] public string Description { get; set; }
        [NotMapped] [Ignore] [BsonIgnore] public string DisplayName { get; set; }

        public virtual int RelatedKey { get; set; }
        public int ContextKey { get; set; }

        [NotMapped] [BsonIgnore] [JsonIgnore]
        public Context Context
        {
            set => ContextKey = value.Key;
        }
        [NotMapped] [BsonIgnore] [Ignore] [JsonIgnore] public virtual List<int> RelatedKeys { get; set; } = new();
        [NotMapped] [BsonIgnore] [Ignore] [JsonIgnore] public IEnumerable RelatedEntities { get; set; }
        public void Link(Base linked)
        {
            ((Subject) linked).SubjectType = Const.Subject.Group;
        }

        public IOneToManyItem<TEntity, TLinked> MakeOneToManyItem<TEntity, TLinked>(TEntity relation, TLinked related)
            where TEntity : Base, IOneToMany
            where TLinked : Base
        {
            return new GroupOneToManyItem<TEntity, TLinked>()
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
            var result = new GroupOneToManyRecord<TEntity, TLinked>()
            {
                Relation = relation,
                Relations = relations
            };
            return result;
        }
    }

    public class GroupEdit: Group, INoneRepo, IOneToManyEdit
    {
        [JsonIgnore] public override int Key { get; set; }
        [JsonIgnore] public override int RelatedKey { get; set; }
        public new List<int> RelatedKeys { get; set; }
    }

    public class GroupAdd : GroupEdit, IOneToManyAdd
    {

    }

    public class GroupOneToManyItem<TEntity, TLinked> : IOneToManyItem<TEntity, TLinked>
        where TEntity : Base, IOneToMany
        where TLinked : Base
    {
        public TEntity Relation { get; set; }

        public TLinked Related { get; set; }
    }

    public class GroupOneToManyRecord<TEntity, TLinked> : IOneToManyRecord<TEntity, TLinked>
        where TEntity : Base, IOneToMany
        where TLinked : Base
    {
        [JsonPropertyName("subject")]
        public TLinked Relation { get; set; }

        public IEnumerable<IOneToManyItem<TEntity, TLinked>> Relations { get; set; }
            = new List<GroupOneToManyItem<TEntity, TLinked>>() { };
    }
}
