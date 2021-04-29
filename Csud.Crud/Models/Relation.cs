using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Entities;

namespace Csud.Crud.Models
{
    public class Relation: Base
    {
#if (Postgre)
        public Context Context { get; set; }
#else
        public One<Context> Context { get; set; }
#endif
    }
}
