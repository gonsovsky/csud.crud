using Csud.Crud.Models.Contexts;

namespace Crud.Csud.RestApi.Models
{
    public class RelationalModel: BaseContext
    {
        public int[] RelatedKeys { get; set; }
    }
}
