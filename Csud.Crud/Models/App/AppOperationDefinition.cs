using Csud.Crud.Models.Internal;

namespace Csud.Crud.Models.App
{
    public class AppOperationDefinition: AppBase, IDisplayNamed
    {
        public int ObjectKey { get; set; }

        public string OperationName { get; set; }

        public string DisplayName { get; set; }

        public int OperationId { get; set; }

        protected override string QueueName => nameof(AppOperationDefinition);
    }
}
