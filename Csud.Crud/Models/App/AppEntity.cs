namespace Csud.Crud.Models.App
{
    public class AppEntity: AppBase
    {
        public int DistribKey { get; set; }

        public string EntityName { get; set; }

        public int EntityKey { get; set; }

        protected override string QueueName => nameof(AppEntity);
    }
}
