using MongoDB.Entities;

namespace Csud.Crud.Models.Internal
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
