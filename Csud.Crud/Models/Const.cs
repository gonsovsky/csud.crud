namespace Csud.Crud.Models
{
    public struct CsudKey
    {
        private int peer;

        public static implicit operator CsudKey(int i)
        {
            return new CsudKey { peer = i };
        }

        public static implicit operator int(CsudKey p)
        {
            return p.peer;
        }
    }

    public static class Const
    {
        public static class Status
        {
            public const string Actual = "actual";
            public const string Removed = "removed";
        }

        public static class Subject
        {
            public const string Account = "account";
            public const string Group = "group";
        }

        public static class Context
        {
            public const string Time = "time";
            public const string Attrib = "attrib";
            public const string Rule = "rule";
            public const string Struct= "struct";
            public const string Segment = "segment";
            public const string Composite = "composite";
        }

        public static class Object
        {
            public const string Role = "role";
            public const string Entity = "entity";
            public const string Task = "task";
        }
    }
}
