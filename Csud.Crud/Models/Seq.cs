using MongoDB.Entities;

namespace Csud.Crud.Models
{
    public class Seq: Entity
    {
        public int Key { get; set; }

        public override string GenerateNewID()
        {
            return GetType().Name;
        }
    }
}
