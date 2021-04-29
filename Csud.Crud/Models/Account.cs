namespace Csud.Crud.Models
{
    public class Account: Base
    {
        public int? AccountProviderKey { get; set; }
        public int? PersonKey { get; set; }
        public int? SubjectKey { get; set; }

        public AccountProvider AccountProvider
        {
            set => AccountProviderKey = value.Key;
        }

        public Subject Subject
        {
            set => SubjectKey = value.Key;
        }

        public Person Person
        {
            set => PersonKey = value.Key;
        }
    }
}
