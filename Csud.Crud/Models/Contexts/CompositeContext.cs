using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Models.Contexts
{
    public class CompositeContext : BaseContext, IRelatable
    {
        public int? RelatedKey { get; set; }

        [NotMapped]
        [BsonIgnore]
        [JsonIgnore]
        public BaseContext RelatedContext
        {
            set => RelatedKey = value.Key;
        }

        [NotMapped] [BsonIgnore] [JsonIgnore] 
        protected List<BaseContext> _RelatedContexts = new List<BaseContext>();

        public void Compose(BaseContext co)
        {
            _RelatedContexts.Add(co);
        }

        public IEnumerable<CompositeContext> Decompose()
        {
            foreach (var c in _RelatedContexts)
            {
                var x = (CompositeContext) this.Clone(true);
                x.RelatedContext = c;
                yield return x;
            }
        }

        [NotMapped] [BsonIgnore] [JsonIgnore] public IEnumerable RelatedContexts { get; set; }

    }
}
