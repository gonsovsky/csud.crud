using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
            //if (value is RelationDetails details)
            //{
              
            //    if (!Csud.Object.Any(x => x.Key == details.ObjectKey))
            //    {
            //        Error($"Неверный код объекта отношений. {details.ObjectKey}");
            //    }
            //    if (!Csud.Subject.Any(x => x.Key == details.SubjectKey))
            //    {
            //        Error($"Неверный код субъекта отношений. {details.SubjectKey}");
            //    }

            //}
            return Validated;
        }
    }

    [RelationDetailsValidator]
    public class RelationDetails : Base, IOneToMany, INameable
    {
        public virtual int RelatedKey { get; set; }

        [NotMapped] [Ignore] [BsonIgnore] public List<int> RelatedKeys { get; set; }
        [NotMapped] [Ignore] [BsonIgnore] [JsonIgnore] public IEnumerable RelatedEntities { get; set; }

        public void Link(Base linked)
        {
           
        }

        public IOneToManyItem<TEntity, TLinked> MakeOneToManyItem<TEntity, TLinked>(TEntity relation, TLinked related)
            where TEntity : Base, IOneToMany
            where TLinked : Base
        {
            return new RelationDetailsOneToManyItem<TEntity, TLinked>()
            {
                Relation = relation,
                Related = related
            };
        }

        public IOneToManyRecord<TEntity, TLinked> MakeOneToManyRecord<TEntity, TLinked>(TLinked relation,
            IEnumerable<IOneToManyItem<TEntity, TLinked>> relations)
            where TEntity : Base, IOneToMany
            where TLinked : Base
        {
            return new RelationDetailsOneToManyRecord<TEntity, TLinked>()
            {
                Relation = relation,
                Relations = relations
            };
        }

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

    public class RelationDetailsEdit : RelationDetails, INoneRepo, IOneToManyEdit
    {
        [JsonIgnore] public override int Key { get; set; }
        public override string Name { get; set; }
        public override string Description { get; set; }
        public override string DisplayName { get; set; }
    }

    public class RelationDetailsAdd : RelationDetailsEdit, INoneRepo, IOneToManyAdd
    {
        [JsonIgnore] public override int RelatedKey { get; set; }
    }

    public class RelationDetailsOneToManyItem<TEntity, TLinked> : IOneToManyItem<TEntity, TLinked>
        where TEntity : Base, IOneToMany
        where TLinked : Base
    {
        public TEntity Relation { get; set; }

        public TLinked Related { get; set; }
    }

    public class RelationDetailsOneToManyRecord<TEntity, TLinked> : IOneToManyRecord<TEntity, TLinked>
        where TEntity : Base, IOneToMany
        where TLinked : Base
    {
        [JsonPropertyName("relation")]
        public TLinked Relation { get; set; }

        public IEnumerable<IOneToManyItem<TEntity, TLinked>> Relations { get; set; }
            = new List<GroupOneToManyItem<TEntity, TLinked>>() { };
    }
}
