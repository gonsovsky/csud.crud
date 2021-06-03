namespace Csud.Crud.Models.Internal
{
    public interface IBase
    {
        public int Key { get; set; }
    }

    public interface INoneRepo : IBase
    {

    }

    internal interface INamed
    {
        string Name { get; set; }
    }

    internal interface IDescribed
    {
        string Description { get; set; }
    }

    internal interface IDisplayNamed
    {
        string DisplayName { get; set; }
    }

    internal interface IWellNamed: INamed, IDescribed, IDisplayNamed
    {

    }

    internal interface IContextable
    {
        int ContextKey { get; set; }
    }

    public interface IEditable : IBase, INoneRepo
    {

    }

    public interface IAddable : IEditable
    {

    }
}
