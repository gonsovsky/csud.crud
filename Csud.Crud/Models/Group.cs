using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Entities;

namespace Csud.Crud.Models
{
    public class Group: Base
    {
#if (Postgre)
        public Subject Subject { get; set; }
        public Subject RelatedSubject { get; set; }
        public Context Context { get; set; }
#else
        public One<Context> Context { get; set; }
        public One<Subject> Subject { get; set; }
        public One<Subject> RelatedSubject { get; set; }
#endif
    }
}
