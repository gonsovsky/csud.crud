using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Csud.Crud.Models
{
    public class Base: Entity
    {
        //[Ignore] public int Key => int.Parse(ID);

#if (Postgre)
        [Key]
        public new int ID { get; set; }

        [NotMapped]
        public bool HasId => ID != 0;
#else
        protected static string Next<T>() where T : Base
        {
            var q = DB.Collection<T>().AsQueryable();
            if (q.Any() == false)
                return 1.ToString();
            var last = Queryable.OrderByDescending(q, x => x.ID).First();
            return (last.ID + 1).ToString();
        }

        [Ignore]
        public bool HasId => string.IsNullOrEmpty(ID) == false;
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
