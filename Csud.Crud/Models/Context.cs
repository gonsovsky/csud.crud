using System.ComponentModel.DataAnnotations.Schema;
using Csud.Crud.Models.Contexts;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Csud.Crud.Models
{
    public class Context: Base
    {
        public string ContextType { get; set; }
        public bool Temporary { get; set; }
        public int HashCode { get; set; }

        [NotMapped]
        [Ignore]
        [BsonIgnore]
        public BaseContext Details { get; set; }
    }
}
