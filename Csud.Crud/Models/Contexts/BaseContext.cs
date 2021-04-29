namespace Csud.Crud.Models.Contexts
{
    public abstract class BaseContext: Base
    {
        public int? ContextKey { get; set; }
        public Context Context
        {
            set => ContextKey = value.Key;
        }
    }
}
