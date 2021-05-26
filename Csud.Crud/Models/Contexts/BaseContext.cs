using System.ComponentModel.DataAnnotations.Schema;
using Csud.Crud.Services;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Csud.Crud.Models.Contexts
{
    public abstract class BaseContext: Base, INameable, IOneToOne
    {
        protected override string QueueName => "Context";

        [NotMapped] [Ignore] [BsonIgnore] public abstract string ContextType { get; }
        [NotMapped] [Ignore] [BsonIgnore] public bool Temporary { get; set; } = false;
        [NotMapped] [Ignore] [BsonIgnore] public string Name { get; set; }
        [NotMapped] [Ignore] [BsonIgnore] public string Description { get; set; }
        [NotMapped] [Ignore] [BsonIgnore] public string DisplayName { get; set; }
    }
}
