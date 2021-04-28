namespace Csud.Crud.Models
{
    public class Context: Base
    {
        public enum ContextTypeEnum
        {
            None,
            Time,
            Segment

        }

        public ContextTypeEnum Type { get; set; }

        public bool Temporary { get; set; }

        public int HashCode { get; set; }

        public string Description { get; set; }

#if (Postgre)

#else
        public override string GenerateNewID() => Next<Context>();
#endif
    }
}
