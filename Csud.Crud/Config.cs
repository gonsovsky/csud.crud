using System;
using System.Data.Common;
using Microsoft.Extensions.Configuration;

namespace Csud.Crud
{
    public record Config
    {
        public abstract record DbClass
        {
            public bool Enabled { get; set; }
        }

        public record PostgreClass: DbClass
        {
            public string ConnectionString { get; set; }

            public string AdminConnectionString { get; set; }

            public string DbName
            {
                get
                {
                    DbConnectionStringBuilder x = new DbConnectionStringBuilder();
                    x.ConnectionString = ConnectionString;
                    var db = x["Database"].ToString();
                    return db;
                }
            }
        }

        public record MongoClass: DbClass
        {
            public string Host { get; set; }
            public int Port { get; set; }
            public string Db { get; set; }
        }

        public PostgreClass Postgre { get; set; } = new PostgreClass();
        public MongoClass Mongo { get; set; } = new MongoClass();

        public Config(IConfiguration cfg)
        {
            Postgre.Enabled = bool.Parse(cfg["Postgre:Enabled"]) || IsDebugMode;
            Postgre.ConnectionString = cfg["Postgre:ConnectionString"];
            Postgre.AdminConnectionString = cfg["Postgre:AdminConnectionString"];
            Mongo.Enabled = bool.Parse(cfg["Mongo:Enabled"]) || IsDebugMode;
            Mongo.Host = cfg["Mongo:Host"];
            Mongo.Port = int.Parse(cfg["Mongo:Port"]);
            Mongo.Db = cfg["Mongo:Db"];
        }

        public string UseDataBase
        {
            get
            {
                var result = "";
                if (Mongo.Enabled)
                    result = "Mongo";
                if (Postgre.Enabled)
                    if (result != "")
                        result += " & Postgre";
                    else
                        result = "Postgre";
                return result;
            }
        }

        public bool IsDebugMode => Environment.MachineName == "SM-SQL" && 1==2;
    }
}
