using MongoDB.Entities;

namespace Csud.Crud.Models.Contexts
{
    public abstract class BaseContext: Base
    {
#if (Postgre)
        public Context Context { get; set; }
#else
        public One<Context> Context { get; set; }
#endif
    }
}
