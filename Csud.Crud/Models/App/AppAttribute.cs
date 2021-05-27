namespace Csud.Crud.Models.App
{
    public class AppAttribute: AppBase
    {
        public int DistribKey { get; set; }

        public string AttributeType { get; set; }

        public int AttributeKey { get; set; }

        protected override string QueueName => nameof(AppAttribute);
    }
}
