using Csud.Crud.Services;

namespace Csud.Crud.Models.Rules
{
    public class Relation : Base, INameable
    {
        protected override string QueueName => "Relation";

        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
    }
}
