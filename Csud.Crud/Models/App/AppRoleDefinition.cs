using Csud.Crud.Models.Internal;

namespace Csud.Crud.Models.App
{
    public class AppRoleDefinition : AppBase, IDescribed, IDisplayNamed
    {
        public int ObjectKey { get; set; }
        public string RoleName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public int? RoleContext { get; set; }
        public int? RoleRule { get; set; }

        protected override string QueueName => nameof(AppRoleDefinition);
    }
}
