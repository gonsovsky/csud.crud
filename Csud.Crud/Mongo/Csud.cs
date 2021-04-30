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
        private protected static IMongoDatabase Db;
        public CsudMongo(string mongoHost, int mongoPort, string mongoDb)
        {
            DB.InitAsync(mongoDb, mongoHost, mongoPort).Wait();
            Db = DB.Database(mongoDb);
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

        public void AddEntity<T>(T entity) where T : Base
        {
            entity.SaveAsync().Wait();
        }

        public void UpdateEntity<T>(T entity) where T : Base
        {
            entity.SaveAsync().Wait();
        }

        public IQueryable<T> Q<T>() where T : Base
        {
            return DB.Queryable<T>();
        }
    }
}
