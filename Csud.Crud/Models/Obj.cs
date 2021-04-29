using MongoDB.Entities;

namespace Csud.Crud.Models
{
    public class Obj : Base
    {
        public enum ObjectType
        {
            None,
            Role,
            Entity,
            Task
        }

        public ObjectType Type { get; set; }

#if (Postgre)
        public Context Context { get; set; }
#else
        public One<Context> Context { get; set; }
#endif
    }
}
