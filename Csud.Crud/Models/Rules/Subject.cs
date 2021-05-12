using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Models.Rules
{
    internal class SubjectValidationAttribute : BaseValidator
    {
        public override bool IsValid(object value)
        {
            Reset();
            var subject = (Subject)value;
            if (!Csud.Context.Any (x => x.Key == subject.ContextKey))
            {
                Error("Неверный код контекста.");
            }
            if (subject.SubjectType != Const.Subject.Account && subject.SubjectType != Const.Subject.Group)
            {
                Error("Неверный тип субъекта.");
            }
            return Validated;
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
