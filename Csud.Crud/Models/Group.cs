namespace Csud.Crud.Models
{
    public class Group: Base
    {
        public int? SubjectKey { get; set; }
        public int? RelatedSubjectKey { get; set; }
        public int? ContextKey { get; set; }

        //public Subject Subject
        //{
        //    set => SubjectKey = value.Key;
        //}
        //public Subject RelatedSubject { get; set; }
        //public Context Context
        //{
        //    set => ContextKey = value.Key;
        //}
    }
}
