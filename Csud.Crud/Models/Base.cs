using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Csud.Crud.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Csud.Crud.Models
{
    public class Base : IEntity, IBase
    { 
        [Key] public virtual int Key { get; set; }

        [BsonId] [AsObjectId] [NotMapped] [JsonIgnore]
        public virtual string ID { get; set; }

        /// <summary>
        /// Override this method in order to control the generation of IDs for new entities.
        /// </summary>
        public virtual string GenerateNewID() => ObjectId.GenerateNewId().ToString();

        public int GenerateNewKey()
        {
            var col = GetType().Name;
            var q = DB.Collection<Seq>().AsQueryable().Where(x => x.ID == col);
            if (q.Any() == false)
            {
                var sq = new Seq() {ID = col, Key = 1};
                sq.SaveAsync().Wait();
                return 1;
            }
            else
            {
                var sq = q.First();
                sq.Key += 1;
                sq.SaveAsync().Wait();
                return sq.Key;
            }
        }

        public virtual T CloneTo<T>(bool withKey, bool skipNull) where T: Base
        {
            var p = Activator.CreateInstance<T>();
            CopyTo(p, withKey, skipNull);
            return p;
        }

        public virtual Base Combine(Base linked, bool skipNull)
        {
            linked.CopyTo(this,false, true);
            return this;
        }

        public virtual void CopyTo(Base destination, bool withKey, bool skipNull)
        {
            var source = this;
            if (source == null || destination == null)
                throw new Exception("Source or/and Destination Objects are null");
            var typeDest = destination.GetType();
            var typeSrc = source.GetType();
            var results = from srcProp in typeSrc.GetProperties()
                let targetProperty = typeDest.GetProperty(srcProp.Name)
                where srcProp.CanRead
                      && targetProperty != null
                      && targetProperty.GetSetMethod(true) != null && !targetProperty.GetSetMethod(true).IsPrivate
                      && (targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) == 0
                      && targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType)
                select new { sourceProperty = srcProp, targetProperty = targetProperty };
            foreach (var props in results)
            {
                if (!withKey)
                {
                    if (props.sourceProperty.Name == nameof(Key) || props.sourceProperty.Name == nameof(ID))
                    {
                        continue;
                    }
                }
                if (props.sourceProperty.Name==nameof(UseKey))
                    continue;

                var newval = props.sourceProperty.GetValue(source, null);

                if (skipNull)
                {
                    if (newval == null)
                        continue;
                    if (props.sourceProperty.PropertyType == typeof(int))
                    {
                        if ((int) newval == 0)
                            continue;
                    }
                }

                props.targetProperty.SetValue(destination, newval, null);
            }

            if (destination is IOneToManyEdit edit && source is IOneToManyEdit manyEdit)
            {
                edit.RelatedKeys = new List<int>();
                edit.RelatedKeys.AddRange(manyEdit.RelatedKeys);
            }
        }
 
        public object Clone(bool keepKey, bool skipnull)
        {
            var x = (Base)Activator.CreateInstance(GetType());
            CopyTo(x, keepKey, skipnull);
            return x;
        }

        [NotMapped]
        [JsonIgnore]
        [BsonIgnore]
        [Ignore]
        public virtual int UseKey
        {
            get => Key;
            set => Key = value;
        }

        public  void Dump()
        {
            string json = JsonSerializer.Serialize(this,new JsonSerializerOptions(){WriteIndented = true});
            Console.WriteLine(json);
        }

        [JsonIgnore]
        public virtual string Status { get; set; } = Const.Status.Actual;
        public virtual void Validate()
        {
            if (Status != Const.Status.Actual && Status != Const.Status.Removed)
                Status = Const.Status.Actual;
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this);
            if (Validator.TryValidateObject(this, context, results, true)) return;
            var err =  string.Join(' ', results.Select(x => x.ErrorMessage));
            throw new ArgumentException(err);
        }
        public virtual bool Look(Base en) => en.UseKey == UseKey;

    }
}
