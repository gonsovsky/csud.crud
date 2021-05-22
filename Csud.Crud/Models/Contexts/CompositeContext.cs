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

        public override string ContextType => Const.Context.Composite;
    }


    [CompositeContextEditValidation]
    public class CompositeContextEdit : CompositeContext, INoneRepo, IOneToManyEdit
    {
        [JsonIgnore] public override int Key { get; set; }
        [JsonIgnore] public override int RelatedKey { get; set; }
        public List<int> RelatedKeys { get; set; }
    }

    public class CompositeContextAdd : CompositeContextEdit, IOneToManyAdd
    {
    }

    internal class CompositeContextEditValidationAttribute : BaseValidator
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Reset();
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
}
