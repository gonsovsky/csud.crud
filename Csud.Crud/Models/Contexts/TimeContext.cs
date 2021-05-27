using System.Text.Json.Serialization;
using Csud.Crud.Models.Internal;
using Csud.Crud.Services;

namespace Csud.Crud.Models.Contexts
{
    public class TimeContext: BaseContext
    {
        public long TimeStart { get; set; }
        public long TimeEnd { get; set; }
        public override string ContextType => Const.Context.Time;
    }

    public class TimeContextEdit : TimeContext, INoneRepo
    {
        [JsonIgnore] public override int Key { get; set; }
    }

    public class TimeContextAdd : TimeContextEdit
    {
    }
}
