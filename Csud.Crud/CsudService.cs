using Microsoft.Extensions.Configuration;

namespace Csud.Crud
{
    public static class CsudService
    {
        public static ICsud Csud;
        public static void StartUp(IConfiguration config)
        {
            var postgre = config["Postgre"];
            var mongoHost = config["Mongo:Host"];
            var mongoPort = config["Mongo:Port"];
            var mongoDb = config["Mongo:Db"];

            Csud = new global::Csud.Crud.Csud(postgre, mongoHost, int.Parse(mongoPort), mongoDb);
        }
    }
}
