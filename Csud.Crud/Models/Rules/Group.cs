using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Models.Rules
{
    internal class GroupValidationAttribute : BaseValidator
    {
        public override bool IsValid(object value)
        {
            Reset();
            var group = (Group)value;
            if (!Csud.Context.Any(x => x.Key == group.ContextKey))
            {
                Error("Неверный код контекста.");
            }
            if (!Csud.Subject.Any(x => x.Key == group.RelatedKey))
            {
                Error("Неверный код связанного субъекта.");
            }
            return Validated;
        }
    }

    [GroupValidation]

    public class Group: Base, IRelatable
    {
        public int? RelatedKey { get; set; }

        [NotMapped]
        [BsonIgnore]
        [JsonIgnore]
        public List<int?> RelatedKeys { get; set; }

        [NotMapped]
        [BsonIgnore]
        [JsonIgnore]
        public IEnumerable RelatedContexts { get; set; }
        public int? ContextKey { get; set; }
    }
}
