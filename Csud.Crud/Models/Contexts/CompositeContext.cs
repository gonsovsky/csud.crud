using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Csud.Crud.Models.Contexts
{
    internal class CompositeValidator : BaseValidator
    {
        public override bool IsValid(object value)
        {
            Reset();
            var composite = value as CompositeContext;
            if (!Csud.Context.Any(x => x.Key == composite.RelatedKey))
            {
                Error("Неверный код связанного объекта.");
            }
            return Validated;
        }
    }

    [CompositeValidator]
    public class CompositeContext : BaseContext, IRelational
    {
        public int RelatedKey { get; set; }
        [NotMapped] [BsonIgnore] [Ignore] [JsonIgnore] public List<int> RelatedKeys { get; set; } = new List<int>();
        [NotMapped] [BsonIgnore] [Ignore] [JsonIgnore] public IEnumerable RelatedEntities { get; set; }
        public void Link(Base linked)
        {
            throw new System.NotImplementedException();
        }
    }
}
