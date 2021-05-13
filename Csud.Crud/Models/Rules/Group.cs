using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Csud.Crud.Models.Rules
{
    internal class GroupValidator : BaseValidator
    {
        public override bool IsValid(object value)
        {
            Reset();
            if (value is IContextable context)
            {
                if (!Csud.Context.Any(x => x.Key == context.ContextKey))
                {
                    Error("Неверный код контекста.");
                }
            }

            if (value is IRelational group)
            {

                if (value is IRelationalAdd)
                {
                    foreach (var rkey in group.RelatedKeys)
                    {
                        if (Csud.Subject.Any(a => a.Key == rkey) == false)
                            Error($"Связанный объект с кодом {rkey} не найден");
                    }
                }
                else
                {
                    if (!Csud.Subject.Any(x => x.Key == group.RelatedKey))
                    {
                        Error("Неверный код связанного объекта.");
                    }
                }
            }

            return Validated;
        }
    }

    [GroupValidator]
    public class Group: Base, IRelational, IContextable
    {
        public virtual int RelatedKey { get; set; }
        public int ContextKey { get; set; }

        [NotMapped] [BsonIgnore] [JsonIgnore]
        public Context Context
        {
            set => ContextKey = value.Key;
        }
        [NotMapped] [BsonIgnore] [Ignore] [JsonIgnore] public virtual List<int> RelatedKeys { get; set; } = new List<int>();
        [NotMapped] [BsonIgnore] [Ignore] [JsonIgnore] public IEnumerable RelatedEntities { get; set; }
        public void Link(Base linked)
        {
            ((Subject) linked).SubjectType = Const.Subject.Group;
        }
    }

    public class GroupAdd : Group, IRelationalAdd
    {
        [JsonIgnore] public override int Key { get; set; }
        [JsonIgnore] public override int RelatedKey { get; set; }
        public new List<int> RelatedKeys { get; set; }
    }
}
