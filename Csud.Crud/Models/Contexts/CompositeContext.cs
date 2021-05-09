using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Models.Contexts
{
    internal class CompositeContextValidationAttribute : BaseValidator
    {
        public override bool IsValid(object value)
        {
            Reset();
            var context = (CompositeContext)value;
            if (!Csud.Context.Any(x => x.Key == context.RelatedKey))
            {
                Error("Неверный код связанного контекста");
            }
            return Validated;
        }
    }

    [CompositeContextValidationAttribute]
    public class CompositeContext : BaseContext, IRelatable
    {
        public int? RelatedKey { get; set; }


        [NotMapped] [BsonIgnore] [JsonIgnore] 
        protected List<int> _RelatedContexts = new List<int>();

        public void Compose(int? co)
        {
            _RelatedContexts.Add((int)co);
        }

        public IEnumerable<CompositeContext> Decompose()
        {
            foreach (var c in _RelatedContexts)
            {
                var x = (CompositeContext) this.Clone(true);
                x.RelatedKey = c;
                yield return x;
            }
        }

        [NotMapped] [BsonIgnore] [JsonIgnore] public IEnumerable RelatedContexts { get; set; }
    }
}
