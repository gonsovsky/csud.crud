namespace Csud.Crud.Models.Contexts
{
    public class RuleContext: BaseContext
    {
#if (Postgre)
#else
        public override string GenerateNewID() => Next<RuleContext>();
#endif
    }
}
