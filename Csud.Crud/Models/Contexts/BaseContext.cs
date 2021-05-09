using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Csud.Crud.Models.Contexts
{
    public abstract class BaseContext: Base
    {
        [NotMapped] [Ignore] [BsonIgnore]
        public string ContextType =>
            GetType().Name.Replace("Context", "").ToLower();
    }
}
