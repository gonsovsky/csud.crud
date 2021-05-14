using Csud.Crud.Models.Contexts;

namespace Csud.Crud.RestApi.Models
{
    public class RelationalModel: BaseContext
    {
        public int[] RelatedKeys { get; set; }
        public override string ContextType { get; }
    }
}
