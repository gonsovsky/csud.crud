namespace Csud.Crud.Models
{
    public class Relation: Base
    {
        public int? ContextKey { get; set; }
        public Context Context
        {
            set => ContextKey = value.Key;
        }
    }
}
