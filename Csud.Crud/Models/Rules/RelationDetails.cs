using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Csud.Crud.Models.Rules
{
    //internal class RelationDetailsValidator : BaseValidator
    //{
    //    public override bool IsValid(object value)
    //    {
    //        Reset();
    //        if (value is RelationDetails group)
    //        {
    //            if (value is IRelationalAdd)
    //            {
    //                foreach (var rkey in group.RelatedKeys)
    //                {
    //                    if (Csud.Select<Relation>().Any(a => a.Key == rkey) == false)
    //                        Error($"Связанный объект с кодом {rkey} не найден");
    //                }
    //            }
    //            else
    //            {
    //                if (!Csud.Select<Relation>().Any(x => x.Key == group.RelatedKey))
    //                {
    //                    Error("Неверный код связанного объекта.");
    //                }
    //            }
    //            if (!Csud.Object.Any(x => x.Key == group.ObjectKey))
    //            {
    //                Error($"Неверный код объекта отношений. {group.ObjectKey}");
    //            }
    //            if (!Csud.Subject.Any(x => x.Key == group.SubjectKey))
    //            {
    //                Error($"Неверный код субъекта отношений. {group.SubjectKey}");
    //            }

    //        }
    //        return Validated;
    //    }
    //}

    //[RelationDetailsValidator]
    //public class RelationDetails : Base, IRelational, INameable
    //{
    //    public int SubjectKey { get; set; }
    //    public int ObjectKey { get; set; }

    //    [NotMapped] [Ignore] [BsonIgnore] [JsonIgnore]
    //    public Subject Subject
    //    {
    //        set => SubjectKey = value.Key;
    //    }

    //    [NotMapped] [Ignore] [BsonIgnore] [JsonIgnore]
    //    public ObjectX Object
    //    {
    //        set => ObjectKey = value.Key;
    //    }

    //    public int JoinMode { get; set; }

    //    [NotMapped] [Ignore] [BsonIgnore] public string Name { get; set; }
    //    [NotMapped] [Ignore] [BsonIgnore] public string Description { get; set; }
    //    [NotMapped] [Ignore] [BsonIgnore] public string DisplayName { get; set; }

    //    public int RelatedKey { get; set; }
    //    [NotMapped] [BsonIgnore] [Ignore] [JsonIgnore] public List<int> RelatedKeys { get; set; } = new List<int>();
    //    [NotMapped] [BsonIgnore] [Ignore] [JsonIgnore] public IEnumerable RelatedEntities { get; set; }

    //    public void Link(Base linked)
    //    {
    //       //
    //    }
    //}

    //public class RelationDetailsAdd : RelationDetails, IRelationalAdd
    //{
    //    [JsonIgnore] protected new int Key { get; set; }
    //    [JsonIgnore] protected new int RelatedKey { get; set; }
    //    public new List<int> RelatedKeys { get; set; }
    //}
}
