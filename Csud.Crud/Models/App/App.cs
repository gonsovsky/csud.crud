using Csud.Crud.Models.Internal;

namespace Csud.Crud.Models.App
{
    public class App: AppBase, INamed
    {
        public string Name { get; set; }

        public int LastDistribKey { get; set; }

        public virtual string DisplayName { get; set; }

        protected override string QueueName => nameof(App);
    }
}
