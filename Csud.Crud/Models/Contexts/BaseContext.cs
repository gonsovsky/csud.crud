using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Csud.Crud.Models.Contexts
{
    public abstract class BaseContext: Base
    {
        public int? ContextKey { get; set; }
        public Context Context
        {
            set => ContextKey = value.Key;
        }

        [NotMapped] [Ignore] [BsonIgnore]
        public override string Name { get; set; }

        [NotMapped] [Ignore] [BsonIgnore]
        public override string DisplayName { get; set; }

        [NotMapped] [Ignore] [BsonIgnore]
        public override string Description { get; set; }

        [NotMapped] [Ignore] [BsonIgnore]
        public string ContextType =>
            GetType().Name.Replace("Context", "").ToLower();
    }
}
