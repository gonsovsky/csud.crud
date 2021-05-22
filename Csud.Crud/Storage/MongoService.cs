using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Models.App;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Models.Rules;
using Csud.Crud.Services;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Csud.Crud.Storage
{
    public partial class MongoService: IDbService
    {
        private protected static IMongoDatabase Db;

        private Config config;

        public MongoService(Config cfg): base()
        {
            if (cfg.Mongo.Enabled)
            {
                this.config = cfg;

                if (cfg.DropOnStart)
                    Drop();

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


                DB.Index<Relation>()
                    .Option(o =>
                    {
                        o.Background = false;
                        o.Unique = true;
                    })
                    .Key(a => a.Key, KeyType.Text)
                    .CreateAsync().Wait();

                DB.Index<RelationDetails>()
                    .Option(o =>
                    {
                        o.Background = false;
                        o.Unique = true;
                    })
                    .Key(a => a.RelatedKey, KeyType.Text)
                    .Key(a => a.ObjectKey, KeyType.Text)
                    .Key(a => a.SubjectKey, KeyType.Text)
                    .CreateAsync().Wait();

                DB.Index<App>().Option(o =>
                    {
                        o.Background = false;
                        o.Unique = true;
                    })
                    .Key(a => a.AppKey, KeyType.Text)
                    .CreateAsync().Wait();

                DB.Index<AppDistrib>().Option(o =>
                    {
                        o.Background = false;
                        o.Unique = true;
                    })
                    .Key(a => a.AppKey, KeyType.Text)
                    .Key(a => a.DistribKey, KeyType.Text)
                    .CreateAsync().Wait();

                DB.Index<AppRole>().Option(o =>
                    {
                        o.Background = false;
                        o.Unique = false;
                    })
                    .Key(a => a.DistribKey, KeyType.Text)
                    .Key(a => a.RoleName, KeyType.Text)
                    .CreateAsync().Wait();

                DB.Index<AppRoleDetails>().Option(o =>
                    {
                        o.Background = false;
                        o.Unique = false;
                    })
                    .Key(a => a.RoleKey, KeyType.Text)
                    .Key(a => a.OperationKey, KeyType.Text)
                    .CreateAsync().Wait();

                DB.Index<AppRoleDefinition>().Option(o =>
                    {
                        o.Background = false;
                        o.Unique = true;
                    })
                    .Key(a => a.RoleKey, KeyType.Text)
                    .CreateAsync().Wait();

                DB.Index<AppEntityDefinition>().Option(o =>
                    {
                        o.Background = false;
                        o.Unique = true;
                    })
                    .Key(a => a.EntityKey, KeyType.Text)
                    .CreateAsync().Wait();

                DB.Index<AppEntity>().Option(o =>
                    {
                        o.Background = false;
                        o.Unique = false;
                    })
                    .Key(a => a.DistribKey, KeyType.Text)
                    .Key(a => a.EntityName, KeyType.Text)
                    .CreateAsync().Wait();

                DB.Index<AppAttributeDefinition>().Option(o =>
                    {
                        o.Background = false;
                        o.Unique = true;
                    })
                    .Key(a => a.AttributeKey, KeyType.Text)
                    .CreateAsync().Wait();

                DB.Index<AppAttribute>().Option(o =>
                    {
                        o.Background = false;
                        o.Unique = false;
                    })
                    .Key(a => a.DistribKey, KeyType.Text)
                    .Key(a => a.AttributeType, KeyType.Text)
                    .CreateAsync().Wait();

                DB.Index<AppOperationDefinition>().Option(o =>
                    {
                        o.Background = false;
                        o.Unique = true;
                    })
                    .Key(a => a.OperationKey, KeyType.Text)
                    .CreateAsync().Wait();

                DB.Index<AppOperation>().Option(o =>
                    {
                        o.Background = false;
                        o.Unique = false;
                    })
                    .Key(a => a.DistribKey, KeyType.Text)
                    .Key(a => a.OperationName, KeyType.Text)
                    .CreateAsync().Wait();
            }
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

        protected void RelatedKeyIndex<T>() where T : Base, IOneToMany
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

        public void Drop()
        {
            DB.InitAsync(config.Mongo.Db, config.Mongo.Host, config.Mongo.Port).Wait();
            DB.Database(config.Mongo.Db).Client.DropDatabase(config.Mongo.Db);
        }

        public string GetPath(string filename)
        {
            return Path.Combine(config.Import.Folder, filename);
        }

        public void Add<T>(T entity, bool generateKey = true) where T : Base
        {
            entity.ID = null;
            if (generateKey)
                entity.UseKey = entity.GenerateNewKey();
            entity.SaveAsync().Wait();
        }

        public void Update<T>(T entity) where T : Base
        {
            entity.SaveAsync().Wait();
        }

        public IQueryable<T> Select<T>(string status = Const.Status.Actual) where T : Base
        {
            return DB.Queryable<T>().Where(x => status== Const.Status.Any || x.Status == status);
        }
    }
}
