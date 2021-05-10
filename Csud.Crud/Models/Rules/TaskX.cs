using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Models.Rules
{
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
        public IEnumerable RelatedContexts { get; set; }

        public int? ObjectKey { get; set; }

        [NotMapped]
        [JsonIgnore]
        [BsonIgnore]
        public ObjectX RelatedObject
        {
            set => RelatedKey = value.Key;
        }
    }
}
