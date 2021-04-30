namespace Csud.Crud.Models
{
    public class Subject: Base
    {
        public string SubjectType { get; set; } = Const.SubjectAccount;
        public int? ContextKey { get; set; }
        public Context Context
        {
            set => ContextKey = value.Key;
        }
    }
}
