using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Models.Rules
{
    internal class TaskValidationAttribute : BaseValidator
    {
        public override bool IsValid(object value)
        {
            Reset();
            var task = (TaskX)value;
            if (!Csud.Context.Any(x => x.Key == task.RelatedKey))
            {
                Error("Неверный код связанного контекста");
            }
            return Validated;
        }
    }

    [TaskValidationAttribute]
    public class TaskX : Base, IRelatable
    {
        public int? RelatedKey { get; set; }

        [NotMapped]
        [BsonIgnore]
        [JsonIgnore]
        public List<int?> RelatedKeys { get; set; }

        [NotMapped]
        [BsonIgnore]
        [JsonIgnore]
        public IEnumerable RelatedEntities { get; set; }
    }
}
