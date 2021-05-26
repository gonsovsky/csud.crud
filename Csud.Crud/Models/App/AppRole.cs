using System;

namespace Csud.Crud.Models.App
{
    public class AppRole : AppBase
    {
        public string DisplayName { get; set; }
        public int DistribKey { get; set; }

        public string RoleName { get; set; }

        protected override string QueueName => nameof(AppRole);
    }
}
