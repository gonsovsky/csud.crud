using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csud.Crud.Models
{
    public class RelationDetails: Base
    {
#if (Postgre)
        public Object Object { get; set; }
        public Subject Subject { get; set; }
#else
        public One<Object> Object { get; set; }
        public One<Subject> Subject { get; set; }
        public override string GenerateNewID() => Next<RelationDetails>();
#endif
    }
}
