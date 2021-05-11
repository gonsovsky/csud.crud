using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Csud.Crud.Models
{
    public class Base : Entity, ICloneable
    { 
        [Key] public virtual int? Key { get; set; }
        [NotMapped] [JsonIgnore]
        public override string ID { get; set; }
        public int? GenerateNewKey()
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
        public virtual void CopyTo(Base destination, bool withKey)
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
                    if (props.sourceProperty.Name == nameof(Base.Key) || props.sourceProperty.Name == nameof(Base.ID))
                    {
                        continue;
                    }
                }
                props.targetProperty.SetValue(destination, props.sourceProperty.GetValue(source, null), null);
            }
        }
        public object Clone()
        {
            var x = (Base)Activator.CreateInstance(this.GetType());
            this.CopyTo(x, false);
            return x;
        }
        public object Clone(bool keepKey)
        {
            var x = (Base)Activator.CreateInstance(this.GetType());
            this.CopyTo(x, keepKey);
            return x;
        }

        public  void Dump()
        {
            string json = JsonSerializer.Serialize(this,new JsonSerializerOptions(){WriteIndented = true});
            Console.WriteLine(json);
        }

        [JsonIgnore]
        public virtual string Status { get; set; } = Const.Status.Actual;
        internal ICsud Csud => CsudService.Csud;
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
    }

  
}
