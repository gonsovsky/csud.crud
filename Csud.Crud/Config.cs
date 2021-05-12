using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
            Postgre.Enabled = bool.Parse(cfg["Postgre:Enabled"]);
            Postgre.ConnectionString = cfg["Postgre:ConnectionString"];
            Mongo.Enabled = bool.Parse(cfg["Mongo:Enabled"]);
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

        public bool IsDebugMode => Environment.MachineName == "SM-SQL";
    }
}
