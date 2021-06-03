using System.Text.Json.Serialization;
using Csud.Crud.Models.Internal;

namespace Csud.Crud.Models.Contexts
{
    public class RuleContext: BaseContext
    {
        public string RuleName { get; set; }

        public override string ContextType => Const.Context.Rule;
    }

    public class RuleContextEdit : RuleContext, INoneRepo
    {
        [JsonIgnore] public override int Key { get; set; }
    }

    public class RuleContextAdd : RuleContextEdit, INoneRepo
    {
    }
}
