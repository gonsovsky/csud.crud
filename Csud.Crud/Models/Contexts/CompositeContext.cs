using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Csud.Crud.Models.Contexts
{
    public class CompositeContext : BaseContext, IRelatable
    {
        public int? RelatedKey { get; set; }

        public BaseContext RelatedContext
        {
            set => RelatedKey = value.Key;
        }

        [NotMapped] [BsonIgnore] private List<BaseContext> RelatedContexts = new List<BaseContext>();

        public void Compose(BaseContext co)
        {
            RelatedContexts.Add(co);
        }

        public IEnumerable<CompositeContext> Decompose()
        {
            foreach (var c in RelatedContexts)
            {
                var x = (CompositeContext)this.Clone(true);
                x.RelatedContext = c;
                yield return x;
            }
        }
    }
}
