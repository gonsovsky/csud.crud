using System.IO;
using Csud.Crud.Models.Maintenance;
using Csud.Crud.Storage;
using MongoDB.Entities;
using Npgsql;

namespace Csud.Crud.Services
{
    public interface IMaintenanceService : IEntityService<AppImport>
    {
        public string GetPath(string filename);
    }

    public class MaintenanceService : EntityService<AppImport>, IMaintenanceService
    {
        protected Config Config;

        public MaintenanceService(Config cfg, IDbService dbSvc) :  base(dbSvc)
        {
            Config = cfg;
        }

        public string GetPath(string filename)
        {
            return Path.Combine(Config.Import.Folder, filename);
        }

        public void Drop()
        {
            if (Config.Mongo.Enabled)
                DropMongo();
            if (Config.Postgre.Enabled)
                DropPostgre();
        }

        public void DropMongo()
        {
            DB.InitAsync(Config.Mongo.Db, Config.Mongo.Host, Config.Mongo.Port).Wait();
            DB.Database(Config.Mongo.Db).Client.DropDatabase(Config.Mongo.Db);
        }

        public void DropPostgre()
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
                     WHERE datname = '{Config.Postgre.Db}';");

            Cmd($" DROP DATABASE IF EXISTS {Config.Postgre.Db};");
        }
    }
}
