using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Csud.Crud.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Services
{
    public interface IOneToMany
    {
        public int RelatedKey { get; set; }
        [NotMapped] [BsonIgnore] [JsonIgnore] public List<int> RelatedKeys { get; set; }
        [NotMapped] [BsonIgnore] [JsonIgnore] public IEnumerable RelatedEntities { get; set; }

        void Link(Base linked);
    }

    public interface IOneToManyAdd: IOneToMany
    {
    }

    public class OneToManyAggregated<TEntity, TLinked> where TEntity : Base, IOneToMany where TLinked : Base
    {
        public TEntity Group { get; set; }
        public IEnumerable<TEntity> Index { get; set; } = new List<TEntity>();

        public TLinked Subject { get; set; }
        public IEnumerable<TLinked> Relations { get; set; } = new List<TLinked>();
    }
}
