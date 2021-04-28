using System;
using System.Collections.Generic;
using System.Linq;
using Csud.Crud.Models;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Csud.Crud.Mongo
{
    public class CsudMongo: ICsud
    {
        public IMongoDatabase Db;

        public async void StartUp(string ip, string dbName)
        {
            await DB.InitAsync(dbName, ip);
            Db = DB.Database(dbName);
            foreach (var entity in Entities)
            {
                entity.StartUp();
            }
        }

        public IEnumerable<Base> Entities => AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(Base)))
            .Where(type => type.IsAbstract==false)
            .Select(type => Activator.CreateInstance(type) as Base);
    }
}
