using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Models.Rules
{
    public class TaskX : Base, IRelatable
    {
        public int? RelatedKey { get; set; }

        public int? ObjectKey { get; set; }

        [NotMapped]
        [JsonIgnore]
        [BsonIgnore]
        public ObjectX RelatedObject
        {
            set => RelatedKey = value.Key;
        }

        [NotMapped]
        [JsonIgnore]
        [BsonIgnore]
        public ObjectX Object
        {
            set => ObjectKey = value.Key;
        }

  
    }
}
