using System;

namespace Csud.Crud.Models.Contexts
{
    public class TimeContext: BaseContext
    {
        public TimeSpan TimeStart { get; set; }

        public TimeSpan TimeEnd { get; set; }

#if (Postgre)
#else
        public override string GenerateNewID() => Next<TimeContext>();
#endif
    }
}
