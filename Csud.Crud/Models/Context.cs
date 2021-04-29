namespace Csud.Crud.Models
{
    public class Context: Base
    {
        public string ContextType { get; set; }
        public bool Temporary { get; set; }
        public int HashCode { get; set; }
    }
}
