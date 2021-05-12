namespace Csud.Crud.Models
{
    internal interface INameable
    {
        string Name { get; set; }
        string Description { get; set; }
        string DisplayName { get; set; }
    }

    internal interface IContextable
    {
        int ContextKey { get; set; }
    }
}
