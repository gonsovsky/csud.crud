namespace Csud.Crud.Models.Contexts
{
    public class CompositeContext : BaseContext
    {
        public int? RelatedKey { get; set; }

        public Context RelatedContext
        {
            set => RelatedKey = value.Key;
        }
    }
}
