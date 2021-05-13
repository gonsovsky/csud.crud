using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Models
{
    public interface IRelational
    {
        public int RelatedKey { get; set; }
        [NotMapped] [BsonIgnore] [JsonIgnore] public List<int> RelatedKeys { get; set; }
        [NotMapped] [BsonIgnore] [JsonIgnore] public IEnumerable RelatedEntities { get; set; }

        void Link(Base linked);
    }

    public interface IRelationalAdd: IRelational
    {
    }
}
