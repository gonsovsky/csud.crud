using Csud.Crud.Models.Internal;

namespace Csud.Crud.Models.App
{
    public class AppRole : AppBase, IDescribed, IDisplayNamed
    {
        public string DisplayName { get; set; }

        public int DistribKey { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }

        public int? RoleContext { get; set; }
        public int? RoleRule { get; set; }

        protected override string QueueName => nameof(AppRole);
    }
}
