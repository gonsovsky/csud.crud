using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using Csud.Crud.Services;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Models.Rules
{
    internal class SubjectValidationAttribute : BaseValidator
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Reset();
            var subject = (Subject)value;
            var service = (IContextService)validationContext.GetService(typeof(IContextService));
            if (service == null)
                throw new ApplicationException($"{nameof(IContextService)} is not found");

            if (value is IAddable || FieldDefined(subject.ContextKey))
            {
                if (!service.Select<Context>().Any(x => x.Key == subject.ContextKey))
                {
                    return new ValidationResult("Неверный код контекста.");
                }
            }

            if (value is IAddable || FieldDefined(subject.SubjectType))
            {
                if (!Const.Subject.Has(subject.SubjectType))
                {
                    return new ValidationResult("Неверный тип субъекта.");
                }
            }

            return null;
        }
    }

    [SubjectValidation]
    public class Subject: Base, INameable
    {
        public string SubjectType { get; set; } = Const.Subject.Account;
        public int ContextKey { get; set; }

        [NotMapped] [JsonIgnore] [BsonIgnore]
        public Context Context
        {
            set => ContextKey = value.Key;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
    }

    public class SubjectEdit : Subject, IEditable
    {
        [JsonIgnore]
        public override int Key { get; set; }
    }

    public class SubjectAdd : SubjectEdit, IAddable
    {

    }
}
