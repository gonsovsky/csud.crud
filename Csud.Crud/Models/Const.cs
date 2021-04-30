namespace Csud.Crud.Models
{
    internal struct Id
    {
        private int peer;

        public static implicit operator Id(int i)
        {
            return new Id { peer = i };
        }

        public static implicit operator int(Id p)
        {
            return p.peer;
        }
    }

    public static class Const
    {
        public const string StatusActual = "actual";
        public const string StatusRemoved = "removed";

        public const string SubjectAccount = "account";
        public const string SubjectGroup = "group";
    }
}
