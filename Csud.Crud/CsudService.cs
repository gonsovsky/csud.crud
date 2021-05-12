using Microsoft.Extensions.Configuration;

namespace Csud.Crud
{
    public static class CsudService
    {
        public static ICsud Csud;

        public static Csud CsudObj;
        public static void StartUp(Config config)
        {
            CsudObj = new global::Csud.Crud.Csud(config);
            Csud = CsudObj;
        }
    }
}
