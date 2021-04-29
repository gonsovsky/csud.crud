namespace Csud.Crud.Models
{
    public class RelationDetails: Base
    {
        public int? SubjectKey { get; set; }
        public int? ObjectKey { get; set; }

        public Subject Subject
        {
            set => SubjectKey = value.Key;
        }
        public ObjectX Object
        {
            set => ObjectKey = value.Key;
        }
    }
}
