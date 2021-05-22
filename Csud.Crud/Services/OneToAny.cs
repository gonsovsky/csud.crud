using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Csud.Crud.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Services
{

    public interface IBase
    {
        public int Key { get; set; }
    }

    public interface INoneRepo : IBase
    {

    }

    internal interface INameable
    {
        string Name { get; set; }
        string Description { get; set; }
        string DisplayName { get; set; }
    }

    internal interface IContextable
    {
        int ContextKey { get; set; }
    }

    public interface IOneToAny: IBase
    {

    }

    public interface IEditable : IBase, INoneRepo
    {

    }

    public interface IAddable : IEditable
    {

    }


    public interface IOneToOne: IOneToAny
    {
        public virtual void Link(Base linked)
        {
            this.Key = linked.Key;
        }
    }

    public interface IOneToMany: IOneToAny
    {
        public int RelatedKey { get; set; }

        [NotMapped] [BsonIgnore] [JsonIgnore] public IEnumerable RelatedEntities { get; set; }

        public void Link(Base linked);
    }

    public interface IOneToManyEdit: IOneToMany, IEditable
    {
        [NotMapped] [BsonIgnore] [JsonIgnore] public List<int> RelatedKeys { get; set; }
    }

    public interface IOneToManyAdd : IOneToManyEdit, IAddable
    {

    }

    public class OneToMany<TEntity, TLinked> where TEntity : Base, IOneToMany where TLinked : Base
    {
        public virtual TEntity Group { get; set; }
        public virtual IEnumerable<TEntity> Index { get; set; } = new List<TEntity>();
        public virtual TLinked Subject { get; set; }
        public virtual IEnumerable<TLinked> Relations { get; set; } = new List<TLinked>();
    }
}
