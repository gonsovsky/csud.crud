namespace Csud.Crud.Models.Contexts
{
    public class RuleContext: BaseContext
    {
        public string RuleName { get; set; }

        public override string ContextType => Const.Context.Rule;
    }
}
