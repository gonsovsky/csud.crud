namespace Csud.Crud.Models.Contexts
{
    public class StructContext: BaseContext
    {
        public string StructCode { get; set; }

        public override string ContextType => Const.Context.Struct;
    }
}
