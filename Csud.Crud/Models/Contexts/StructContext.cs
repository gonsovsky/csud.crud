namespace Csud.Crud.Models.Contexts
{
    public class StructContext: BaseContext
    {
        public string StructCode { get; set; }

#if (Postgre)
#else
        public override string GenerateNewID() => Next<StructContext>();
#endif
    }
}
