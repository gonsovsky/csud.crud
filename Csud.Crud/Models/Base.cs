using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Csud.Crud.Mongo;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Csud.Crud.Models
{
    public class Base: Entity
    {
#if (Postgre)
        [Key]
        public new int ID { get; set; }

        [NotMapped]
        public bool HasId => ID != 0;
#else
        public override string GenerateNewID()
        {
            var col = GetType().Name;
            var q = DB.Collection<Seq>().AsQueryable().Where(x => x.ID == col);
            if (q.Any() == false)
            {
                var sq = new Seq() { ID = col, Key = 1 };
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

        [Ignore] public bool HasId => !string.IsNullOrEmpty(ID);
#endif

        public virtual void StartUp()
        {
        }

        public virtual ItemStatus Status { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string DisplayName { get; set; }
    }

    public enum ItemStatus
    {
        Actual,
        Removed
    }
}
