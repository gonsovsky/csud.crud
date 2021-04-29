using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Csud.Crud.Models
{
    public class Base : IEntity
    {
        [Key] [BsonIgnore] public int? Key { get; set; }

        [NotMapped] [BsonElement("Key")] 
        public string ID
        {
            get => Key.ToString();
            set => Key = int.Parse(value);
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
