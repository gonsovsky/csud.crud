namespace Csud.Crud.Models.Rules
{
    public class Relation : Base, INameable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
    }
}
