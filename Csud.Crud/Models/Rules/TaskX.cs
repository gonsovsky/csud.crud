using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using Csud.Crud.Services;
using MongoDB.Bson.Serialization.Attributes;

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
    public class TaskX : Base, IOneToMany
    {
        public int RelatedKey { get; set; }
        [NotMapped] [BsonIgnore][JsonIgnore] public IEnumerable RelatedEntities { get; set; }
        public void Link(Base linked)
        {
            ((ObjectX)linked).ObjectType = Const.Object.Task;
        }
    }

    public class TaskEdit : TaskX, IOneToManyEdit
    {
        [JsonIgnore] protected new int Key { get; set; }
        [JsonIgnore] protected new int RelatedKey { get; set; }
        public virtual List<int> RelatedKeys { get; set; }
    }

    public class TaskAdd : TaskEdit, IOneToManyAdd
    {

    }
}
