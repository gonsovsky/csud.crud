using Csud.Crud.Models.Internal;

namespace Csud.Crud.Models.App
{
    public class AppOperation: AppBase, IDisplayNamed
    {
        public override int Key { get; set; }

        public int DistribKey { get; set; }

        public string OperationName { get; set; }

        public string DisplayName { get; set; }

        public int OperationId { get; set; }

        protected override string QueueName => nameof(AppOperation);
    }
}
