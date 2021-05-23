﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Csud.Crud.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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

        public IOneToManyItem<TEntity, TLinked> MakeOneToManyItem<TEntity, TLinked>(TEntity relation, TLinked related)
            where TEntity : Base, IOneToMany
            where TLinked : Base;

        public IOneToManyRecord<TEntity, TLinked> MakeOneToManyRecord<TEntity, TLinked>(TLinked relation, IEnumerable<IOneToManyItem<TEntity, TLinked>> relations)
            where TEntity : Base, IOneToMany
            where TLinked : Base;
    }

    public interface IOneToManyEdit: IOneToMany, IEditable
    {
        [NotMapped] [BsonIgnore] [JsonIgnore] public List<int> RelatedKeys { get; set; }
    }

    public interface IOneToManyAdd : IOneToManyEdit, IAddable
    {

    }

    public interface IOneToManyItem<TEntity, TLinked> 
        where TEntity : Base, IOneToMany 
        where TLinked : Base
    {
        [JsonPropertyName("relation")]
        TEntity Relation { get; set; }

        [JsonPropertyName("related")]
        TLinked Related { get; set; }
    }

    public interface IOneToManyRecord<TEntity, TLinked>
        where TEntity : Base, IOneToMany 
        where TLinked : Base
    {
        [JsonPropertyName("relation")]
        TLinked Relation { get; set; }

        [JsonPropertyName("relations")]
        IEnumerable<IOneToManyItem<TEntity, TLinked>> Relations { get; set; }
    }
}
