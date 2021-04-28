namespace Csud.Crud.Models.Contexts
{
    public class CompositeContext : BaseContext
    {
#if (Postgre)
        public Context RelatedContext { get; set; }
#else
        public One<Context> RelatedContext { get; set; }
        public override string GenerateNewID() => Next<CompositeContext>();
#endif
    }
}
