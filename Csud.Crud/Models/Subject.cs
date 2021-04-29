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
        public int? ContextKey { get; set; }
        public Context Context
        {
            set => ContextKey = value.Key;
        }
    }
}
