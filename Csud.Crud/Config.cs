using System;
using System.Data.Common;
using Microsoft.Extensions.Configuration;

namespace Csud.Crud
{
    public record Config
    {
        public bool DropOnStart { get; set; }

        public record ImportClass
        {
            public string Folder { get; set; }
        }

        public abstract record DbClass
        {
            public bool Enabled { get; set; }
        }

        public record PostgreClass: DbClass
        {
            public string ConnectionString { get; set; }

            public string AdminConnectionString { get; set; }

            public string Db
            {
                get
                {
                    var x = new DbConnectionStringBuilder {ConnectionString = ConnectionString};
                    var val = x["Database"].ToString();
                    return val;
                }
            }

            public string Host
            {
                get
                {
                    var x = new DbConnectionStringBuilder { ConnectionString = ConnectionString };
                    var val = x["Server"].ToString();
                    return val;
                }
            }

            public string Port
            {
                get
                {
                    var x = new DbConnectionStringBuilder { ConnectionString = ConnectionString };
                    var val = x["Port"].ToString();
                    return val;
                }
            }
        }

        public record MongoClass: DbClass
        {
            public string Host { get; set; }
            public int Port { get; set; }
            public string Db { get; set; }
        }

        public ImportClass Import { get; set; } = new();
        public PostgreClass Postgre { get; set; } = new();
        public MongoClass Mongo { get; set; } = new();

        public Config(IConfiguration cfg)
        {
            Postgre.Enabled = bool.Parse(cfg["Postgre:Enabled"])|| IsDebugMode;
            Postgre.ConnectionString = cfg["Postgre:ConnectionString"];
            Postgre.AdminConnectionString = cfg["Postgre:AdminConnectionString"];
            Mongo.Enabled = bool.Parse(cfg["Mongo:Enabled"]) || IsDebugMode;
            Mongo.Host = cfg["Mongo:Host"];
            Mongo.Port = int.Parse(cfg["Mongo:Port"]);
            Mongo.Db = cfg["Mongo:Db"];
            Import.Folder = cfg["Import:Folder"];
        }

        public string UseDataBase
        {
            get
            {
                var result = "";
                if (Mongo.Enabled)
                    result = "Mongo";
                if (!Postgre.Enabled) return result;
                if (result != "")
                    result += " & Postgre";
                else
                    result = "Postgre";
                return result;
            }
        }

        public bool IsDebugMode => Environment.MachineName == "SM-SQL";
    }
}
