using System.Text.Json.Serialization;

namespace Csud.Crud.Models.Contexts
{
    public class TimeContext: BaseContext
    {
        public long TimeStart { get; set; }
        public long TimeEnd { get; set; }
        public override string ContextType => Const.Context.Time;
    }

    public class TimeContextAdd : TimeContext
    {
        [JsonIgnore] public override int Key { get; set; }
    }

    public class TimeContextEdit : TimeContext
    {
    }
}
