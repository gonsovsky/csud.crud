using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Entities;

namespace Csud.Crud.Models
{
    public class RelationDetails: Base
    {
#if (Postgre)
        public Object Object { get; set; }
        public Subject Subject { get; set; }
#else
        public One<Obj> Object { get; set; }
        public One<Subject> Subject { get; set; }
#endif
    }
}
