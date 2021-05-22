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
            ValidationResult result =null;
            var subject = (Subject)value;
            var service = (IContextService)validationContext
                .GetService(typeof(IContextService));

            if (!service.Select<Context>().Any (x => x.Key == subject.ContextKey))
            {
                result = new ValidationResult("Неверный код контекста.");
            }
            if (subject.SubjectType != Const.Subject.Account && subject.SubjectType != Const.Subject.Group)
            {
                result = new ValidationResult("Неверный тип субъекта.");
            }
            return result;
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
}
