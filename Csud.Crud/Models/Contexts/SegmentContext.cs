using System.Text.Json.Serialization;
using Csud.Crud.Services;

namespace Csud.Crud.Models.Contexts
{
    public class SegmentContext : BaseContext
    {
        public string SegmentName { get; set; }

        public override string ContextType => Const.Context.Segment;
    }
    public class SegmentContextEdit : SegmentContext, INoneRepo
    {
        [JsonIgnore] public override int Key { get; set; }
    }

    public class SegmentContextAdd : SegmentContextEdit, INoneRepo
    {
    }
}
