namespace Csud.Crud.Models
{
    public class TaskX : Base
    {
        public int? RelatedObjectKey { get; set; }
        public int? ObjectKey { get; set; }

        public ObjectX RelatedObject
        {
            set => RelatedObjectKey = value.Key;
        }
        public ObjectX Object
        {
            set => ObjectKey = value.Key;
        }
    }
}
