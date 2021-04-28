namespace Csud.Crud.Models
{
    public class AccountProvider : Base
    {
        public string Type { get; set; }

#if (Postgre)
#else
        public override string GenerateNewID() => Next<AccountProvider>();
#endif
    }
}
