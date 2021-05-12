using MongoDB.Entities;
using Npgsql;

namespace Csud.Crud
{
    public static class CsudService
    {
        public static ICsud Csud;

        public static Csud CsudObj;

        public static Config Config;

        public static void StartUp(Config config)
        {
            Config = config;
            CsudObj = new global::Csud.Crud.Csud(config);
            Csud = CsudObj;
        }

        public static void Drop(Config config)
        {
            Config = config;
            if (Config.Mongo.Enabled)
                DropMongo();
            if (Config.Postgre.Enabled)
                DropPostgre();
        }

        public static void DropMongo()
        {
            DB.InitAsync(Config.Mongo.Db, Config.Mongo.Host, Config.Mongo.Port).Wait();
            DB.Database(Config.Mongo.Db).Client.DropDatabase(Config.Mongo.Db);
        }

        public static void DropPostgre()
        {
            void Cmd(string sql)
            {
                using var con = new NpgsqlConnection(Config.Postgre.AdminConnectionString);
                con.Open();
                using var cmd = new NpgsqlCommand(sql, con);
                cmd.ExecuteNonQuery();
            }

            Cmd("SELECT version()");

            Cmd($@"SELECT pg_terminate_backend(pid)
                     FROM pg_stat_activity
                     WHERE datname = '{Config.Postgre.DbName}';");

            Cmd($" DROP DATABASE IF EXISTS {Config.Postgre.DbName};");
        }
    }
}
