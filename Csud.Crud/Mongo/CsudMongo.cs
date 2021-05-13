using System;
using System.Collections.Generic;
using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Models.Rules;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Csud.Crud.Mongo
{
    public partial class CsudMongo: ICsud
    {
        private protected static IMongoDatabase Db;

        public CsudMongo(Config cfg)
        {
            DB.InitAsync(cfg.Mongo.Db, cfg.Mongo.Host, cfg.Mongo.Port).Wait();
            Db = DB.Database(cfg.Mongo.Db);

            RelatedKeyIndex<Group>();
            RelatedKeyIndex<CompositeContext>();
            RelatedKeyIndex<TaskX>();

            KeyIndex<Context>();
            KeyIndex<AccountProvider>();
            KeyIndex<ObjectX>();
            KeyIndex<Person>();
            KeyIndex<Subject>();
            KeyIndex<TimeContext>();
            KeyIndex<SegmentContext>();
            KeyIndex<StructContext>();
            KeyIndex<RuleContext>();

            DB.Index<Person>()
                .Option(o => o.Background = false)
                .Key(a => a.FirstName, KeyType.Text)
                .Key(a => a.LastName, KeyType.Text)
                .CreateAsync().Wait();

            DB.Index<Account>()
                .Option(o =>
                {
                    o.Background = false;
                    o.Unique = true;
                })
                .Key(a => a.Key, KeyType.Text)
                .Key(a => a.AccountProviderKey, KeyType.Text)
                .CreateAsync().Wait();

            AppCsudMongo(cfg);
        }

        protected void KeyIndex<T>() where T: Base
        {
            DB.Index<T>()
                .Option(o =>
                {
                    o.Background = false;
                    o.Unique = true;
                })
                .Key(a => a.Key, KeyType.Text)
                .CreateAsync().Wait();
        }

        protected void RelatedKeyIndex<T>() where T : Base, IRelational
        {
            DB.Index<T>()
                .Option(o =>
                {
                    o.Background = false;
                    o.Unique = true;
                })
                .Key(a => a.Key, KeyType.Text)
                .Key(a => a.RelatedKey, KeyType.Text)
                .CreateAsync().Wait();
        }

        public IEnumerable<Base> Entities => AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(Base)))
            .Where(type => type.IsAbstract==false)
            .Select(type => Activator.CreateInstance(type) as Base);

        public void AddEntity<T>(T entity, bool generateKey=true) where T : Base
        {
            entity.ID = null;
            if (generateKey)
                entity.UseKey = entity.GenerateNewKey();
            entity.SaveAsync().Wait();
        }

        public void UpdateEntity<T>(T entity) where T : Base
        {
            entity.SaveAsync().Wait();
        }

        public IQueryable<T> Select<T>(string status = Const.Status.Actual) where T : Base
        {
            return DB.Queryable<T>().Where(x => x.Status == status);
        }
    }
}
