namespace Csud.Crud.Models.Rules
{
    public class Group: Base, IRelatable
    {
        public int? RelatedKey { get; set; }
        public int? ContextKey { get; set; }
    }
}
