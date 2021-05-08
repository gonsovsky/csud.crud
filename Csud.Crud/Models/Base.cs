using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Csud.Crud.Models
{
    public class Base : IEntity, ICloneable
    {
        [Key] public int? Key { get; set; }

        [NotMapped]
        public string ID
        {
            get => Key.ToString();
            set
            {
                if (!string.IsNullOrEmpty(value))
                    Key = int.Parse(value);
                else
                    Key = null;
            }
        }

        [Ignore] public bool HasId => Key != null && Key != 0;

        public string GenerateNewID()
        {
            var col = GetType().Name;
            var q = DB.Collection<Seq>().AsQueryable().Where(x => x.ID == col);
            if (q.Any() == false)
            {
                var sq = new Seq() {ID = col, Key = 1};
                sq.SaveAsync().Wait();
                return 1.ToString();
            }
            else
            {
                var sq = q.First();
                sq.Key += 1;
                sq.SaveAsync().Wait();
                return sq.Key.ToString();
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
                if (props.sourceProperty.Name == "Key")
                {
                    if (!withKey)
                        continue;
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

        public virtual string Status { get; set; } = Const.StatusActual;
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string DisplayName { get; set; }
     
    }
}
