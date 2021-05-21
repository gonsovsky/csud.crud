using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using Csud.Crud.Services;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Csud.Crud.Models.Rules
{
    internal class RelationDetailsValidator : BaseValidator
    {
        public override bool IsValid(object value)
        {
            Reset();
            if (value is RelationDetails details)
            {
              
                if (!Csud.Object.Any(x => x.Key == details.ObjectKey))
                {
                    Error($"Неверный код объекта отношений. {details.ObjectKey}");
                }
                if (!Csud.Subject.Any(x => x.Key == details.SubjectKey))
                {
                    Error($"Неверный код субъекта отношений. {details.SubjectKey}");
                }

            }
            return Validated;
        }
    }

    [RelationDetailsValidator]
    public class RelationDetails : Base, IOneToOne, INameable
    {
        public virtual int RelatedKey { get; set; }
        public int SubjectKey { get; set; }
        public int ObjectKey { get; set; }
        public int JoinMode { get; set; }

        [NotMapped] [Ignore] [BsonIgnore] public virtual string Name { get; set; }
        [NotMapped] [Ignore] [BsonIgnore] public virtual string Description { get; set; }
        [NotMapped] [Ignore] [BsonIgnore] public virtual string DisplayName { get; set; }

        [NotMapped] [Ignore] [BsonIgnore] [JsonIgnore]
        public Subject Subject
        {
            set => SubjectKey = value.Key;
        }

        [NotMapped] [Ignore] [BsonIgnore] [JsonIgnore]
        public ObjectX Object
        {
            set => ObjectKey = value.Key;
        }
    }

    public class RelationDetailsEdit : RelationDetails
    {
        [JsonIgnore] public override int Key { get; set; }
        public override string Name { get; set; }
        public override string Description { get; set; }
        public override string DisplayName { get; set; }
    }

    public class RelationDetailsAdd : RelationDetailsEdit
    {
        [JsonIgnore] public override int RelatedKey { get; set; }
    }
}
