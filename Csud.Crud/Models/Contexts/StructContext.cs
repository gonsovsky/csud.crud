using System.Text.Json.Serialization;
using Csud.Crud.Services;

namespace Csud.Crud.Models.Contexts
{
    public class StructContext: BaseContext
    {
        public string StructCode { get; set; }

        public override string ContextType => Const.Context.Struct;
    }

    public class StructContextEdit : StructContext, INoneRepo
    {
        [JsonIgnore] public override int Key { get; set; }
    }

    public class StructContextAdd : StructContextEdit, INoneRepo
    {

    }
}
