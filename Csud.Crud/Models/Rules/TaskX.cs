using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using Csud.Crud.Models.Internal;
using Csud.Crud.Services;
using Csud.Crud.Services.Internal;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Csud.Crud.Models.Rules
{
    internal class TaskValidator : BaseValidator
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Reset();
            var service = (IEntityService<ObjectX>) validationContext.GetService(typeof(IEntityService<ObjectX>));
            if (service == null)
                throw new ApplicationException($"{nameof(EntityService<ObjectX>)} is not found");

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

    [TaskValidator]
    public class TaskX : Base, IOneToMany, IWellNamed
    {
        protected override string QueueName => "Task";

        [NotMapped] [Ignore] [BsonIgnore] public string Name { get; set; }
        [NotMapped] [Ignore] [BsonIgnore] public string Description { get; set; }
        [NotMapped] [Ignore] [BsonIgnore] public string DisplayName { get; set; }

        public int RelatedKey { get; set; }
        [NotMapped] [BsonIgnore][JsonIgnore] public IEnumerable RelatedEntities { get; set; }
        public void Link(Base linked)
        {
            ((ObjectX)linked).ObjectType = Const.Object.Task;
        }

        public IOneToManyItem<TEntity, TLinked> MakeOneToManyItem<TEntity, TLinked>(TEntity relation, TLinked related)
            where TEntity : Base, IOneToMany
            where TLinked : Base
        {
            return new TaskOneToManyItem<TEntity, TLinked>()
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
            return new TaskOneToManyRecord<TEntity, TLinked>()
            {
                Relation = relation,
                Relations = relations
            };
        }
    }

    public class TaskXEdit : TaskX, IOneToManyEdit
    {
        [JsonIgnore] protected new int Key { get; set; }
        [JsonIgnore] protected new int RelatedKey { get; set; }
        public virtual List<int> RelatedKeys { get; set; }
    }

    public class TaskXAdd : TaskXEdit, IOneToManyAdd
    {

    }

    public class TaskOneToManyItem<TEntity, TLinked> : IOneToManyItem<TEntity, TLinked>
        where TEntity : Base, IOneToMany
        where TLinked : Base
    {
        public TEntity Relation { get; set; }

        public TLinked Related { get; set; }
    }

    public class TaskOneToManyRecord<TEntity, TLinked> : IOneToManyRecord<TEntity, TLinked>
        where TEntity : Base, IOneToMany
        where TLinked : Base
    {
        [JsonPropertyName("object")]
        public TLinked Relation { get; set; }

        public IEnumerable<IOneToManyItem<TEntity, TLinked>> Relations { get; set; }
            = new List<GroupOneToManyItem<TEntity, TLinked>>() { };
    }
}
