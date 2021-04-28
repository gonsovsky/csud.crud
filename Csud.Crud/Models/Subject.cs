using MongoDB.Entities;

namespace Csud.Crud.Models
{
    public class Subject: Base
    {
        public enum SubjectTypeEnum
        {
            None,
            Account,
            Group
        }

        public SubjectTypeEnum SubjectType { get; set; }

#if (Postgre)
        public Context Context { get; set; }
#else
        public One<Context> Context { get; set; }
        public override string GenerateNewID() => Next<Subject>();
#endif
    }
}
