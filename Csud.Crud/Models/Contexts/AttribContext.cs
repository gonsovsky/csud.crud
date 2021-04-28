namespace Csud.Crud.Models.Contexts
{
    public class AttribContext: BaseContext
    {
#if (Postgre)
#else
        public override string GenerateNewID() => Next<AttribContext>();
#endif
    }
}
