using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Models.Rules
{
    internal class ObjectValidationAttribute : BaseValidator
    {
        public override bool IsValid(object value)
        {
            Reset();
            var obj = (ObjectX)value;
            if (!Csud.Context.Any(x => x.Key == obj.ContextKey))
            {
                Error("Неверный код контекста.");
            }
            if (obj.Type != Const.Object.Role && obj.Type != Const.Object.Task && obj.Type != Const.Object.Entity)
            {
                Error("Неверный тип объекта.");
            }
            return Validated;
        }
    }

    [ObjectValidation]
    public class ObjectX : Base
    {
        public string Type { get; set; } = Const.Object.Role;

        public int? ContextKey { get; set; }

        [NotMapped]
        [BsonIgnore]
        [JsonIgnore]
        public Context Context
        {
            set => ContextKey = value.Key;
        }
    }
}
