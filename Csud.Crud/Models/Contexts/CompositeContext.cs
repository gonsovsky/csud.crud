namespace Csud.Crud.Models.Contexts
{
    public class CompositeContext : BaseContext
    {
        public int? RelatedContextKey { get; set; }

        public Context RelatedContext
        {
            set => RelatedContextKey = value.Key;
        }
    }
}
