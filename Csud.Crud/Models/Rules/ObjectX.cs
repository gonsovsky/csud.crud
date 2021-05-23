using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using Csud.Crud.Services;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Models.Rules
{
    internal class ObjectValidationAttribute : BaseValidator
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Reset();
            var objectX = (ObjectX)value;
            var service = (IContextService)validationContext.GetService(typeof(IContextService));
            if (service == null)
                throw new ApplicationException($"{nameof(IContextService)} is not found");

            if (!service.Select<Context>().Any(x => x.Key == objectX.ContextKey))
            {
                return new ValidationResult("Неверный код контекста.");
            }
            if (!Const.Object.Has(objectX.ObjectType))
            {
                return new ValidationResult("Неверный тип объекта.");
            }

            return null;
        }
    }

    [ObjectValidation]
    public class ObjectX : Base, INameable
    {
        public string ObjectType { get; set; } = Const.Object.Role;

        public int ContextKey { get; set; }

        [NotMapped] [BsonIgnore] [JsonIgnore]
        public Context Context
        {
            set => ContextKey = value.Key;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
    }

    public class ObjectXEdit : ObjectX, IEditable
    {
        [JsonIgnore]
        public override int Key { get; set; }
    }

    public class ObjectXAdd : ObjectXEdit, IAddable
    {

    }
}
