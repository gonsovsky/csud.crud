using System.Text.Json.Serialization;
using Csud.Crud.Services;

namespace Csud.Crud.Models.Contexts
{
    public class AttributeContext : BaseContext
    {
        public override string ContextType => Const.Context.Attrib;
    }
    public class AttributeContextEdit : AttributeContext, INoneRepo
    {
        [JsonIgnore] public override int Key { get; set; }
    }
    public class AttributeContextAdd : AttributeContextEdit
    {

    }
}
