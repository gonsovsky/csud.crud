using MongoDB.Entities;

namespace Csud.Crud.Models
{
    public class Account: Base
    {
#if (Postgre)
        public AccountProvider AccountProvider { get; set; }
        public Subject Subject { get; set; }
        public Person Person { get; set; }

#else
        public One<AccountProvider> AccountProvider { get; set; }
        public One<Subject> Subject { get; set; }
        public One<Person> Person { get; set; }
        public override string GenerateNewID() => Next<Account>();
#endif

        public bool HasProvider => AccountProvider?.HasId == true;

        public bool HasSubject => Subject?.HasId == true;

        public bool HasPerson => Person?.HasId == true;
    }
}
