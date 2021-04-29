using System;
using System.Collections.Generic;
using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
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
        public void AddPerson(Person person) => AddEntity(person);
        public void AddAccountProvider(AccountProvider provider) => AddEntity(provider);
        public void AddAccount(Account account) => AddEntity(account);
        public void AddSubject(Subject subject) => AddEntity(subject);
        public void AddContext(Context context) => AddEntity(context);
        public void AddTimeContext(TimeContext timeContext) => AddEntity(timeContext);
        public void AddSegmentContext(TimeContext segmentContext) => AddEntity(segmentContext);
    }
}
