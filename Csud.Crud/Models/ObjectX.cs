namespace Csud.Crud.Models
{
    public class ObjectX : Base
    {
        public enum ObjectType
        {
            None,
            Role,
            Entity,
            Task
        }

        public ObjectType Type { get; set; }

        public int? ContextKey { get; set; }

        public Context Context
        {
            set => ContextKey = value.Key;
        }
    }
}
