namespace Csud.Crud.Models.App
{
    public class AppOperationDefinition: AppBase
    {
        public int ObjectKey { get; set; }

        public string OperationName { get; set; }

        protected override string QueueName => nameof(AppOperationDefinition);
    }
}
