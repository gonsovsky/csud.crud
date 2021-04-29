using System;

namespace Csud.Crud.Models.Contexts
{
    public class TimeContext: BaseContext
    {
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
    }
}
