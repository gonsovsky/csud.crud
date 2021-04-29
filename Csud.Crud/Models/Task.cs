namespace Csud.Crud.Models
{
    public class TaskX : Base
    {
        public int? RelatedObjectKey { get; set; }
        public int? ObjectKey { get; set; }

        public Obj RelatedObject
        {
            set => RelatedObjectKey = value.Key;
        }
        public Obj Object
        {
            set => ObjectKey = value.Key;
        }
    }
}
