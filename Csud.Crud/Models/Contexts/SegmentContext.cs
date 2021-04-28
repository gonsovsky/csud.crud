namespace Csud.Crud.Models.Contexts
{
    public class SegmentContext : BaseContext
    {
#if (Postgre)
#else
        public override string GenerateNewID() => Next<SegmentContext>();
#endif
    }
}
