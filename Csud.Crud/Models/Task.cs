using MongoDB.Entities;

namespace Csud.Crud.Models
{
    public class TaskX : Base
    {
#if (Postgre)
        public Obj Object { get; set; }
        public Obj RelatedObject { get; set; }
#else
        public One<Obj> Object { get; set; }
        public One<Obj> RelatedObject { get; set; }
        public override string GenerateNewID() => Next<Obj>();
#endif
    }
}
