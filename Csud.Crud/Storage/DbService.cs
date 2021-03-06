using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Csud.Crud.Models;
using Csud.Crud.Models.App;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Models.Internal;
using Csud.Crud.Models.Maintenance;
using Csud.Crud.Models.Rules;
using Csud.Crud.Services.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Csud.Crud.Storage
{
    public interface IDbService
    {
        public void Drop();
        string GetPath(string filename);
        public T Add<T>(T entity, bool generateKey = true) where T : Base;
        public T Get<T>(IEntityKey key) where T : Base;
        public void Update<T>(T entity) where T : Base;
        public void Delete<T>(T entity) where T : Base;
        public IQueryable<T> Select<T>(string status = Const.Status.Actual) where T : Base;
        public IQueryable<Person> Person => Select<Person>();
        public IQueryable<AccountProvider> AccountProvider => Select<AccountProvider>();
        public IQueryable<Account> Account => Select<Account>();
        public IQueryable<Subject> Subject => Select<Subject>();
        public IQueryable<ObjectX> Object => Select<ObjectX>();
        public IQueryable<TaskX> Task => Select<TaskX>();
        public IQueryable<Group> Group => Select<Group>();
        public IQueryable<Context> Context => Select<Context>();

        public IQueryable<TimeContext> TimeContext => Select<TimeContext>();
        public IQueryable<StructContext> StructContext => Select<StructContext>();
        public IQueryable<RuleContext> RuleContext => Select<RuleContext>();
        public IQueryable<SegmentContext> SegmentContext => Select<SegmentContext>();
        public IQueryable<CompositeContext> CompositeContext => Select<CompositeContext>();

        public IQueryable<Relation> Relation => Select<Relation>();
        public IQueryable<RelationDetails> RelationDetails => Select<RelationDetails>();

        public IQueryable<App> App => Select<App>();
        public IQueryable<AppDistrib> AppDistrib => Select<AppDistrib>();
        public IQueryable<AppRole> AppRole => Select<AppRole>();
        public IQueryable<AppRoleDefinition> AppRoleDefinition => Select<AppRoleDefinition>(Const.Status.Any);
        public IQueryable<AppRoleDetails> AppRoleDetails => Select<AppRoleDetails>();
        public IQueryable<AppEntityDefinition> AppEntityDefinition => Select<AppEntityDefinition>();
        public IQueryable<AppEntity> AppEntity => Select<AppEntity>();
        public IQueryable<AppAttributeDefinition> AppAttributeDefinition => Select<AppAttributeDefinition>();
        public IQueryable<AppAttribute> AppAttribute => Select<AppAttribute>();
        public IQueryable<AppOperationDefinition> AppOperationDefinition => Select<AppOperationDefinition>(Const.Status.Any);
        public IQueryable<AppOperation> AppOperation => Select<AppOperation>();
        public IQueryable<AppImport> AppImport => Select<AppImport>();
    }

    public class DbService: IDbService
    {
        public static List<IDbService> DbX = new();

        public static MongoService Mongo;

        public static PostgreService Postgre;

        private Config config;

        public DbService(Config cfg, PostgreService postgre, MongoService mongo)
        {
            config = cfg;
            if (cfg.Mongo.Enabled)
            {
                Mongo = mongo;
                DbX.Add(Mongo);
            }
            if (cfg.Postgre.Enabled)
            {
                Postgre = postgre;
                DbX.Add(Postgre);
            }
        }

        public string GetPath(string filename)
        {
            return Path.Combine(config.Import.Folder, filename);
        }

        public void Drop()
        {
            foreach (var db in DbX)
            {
                db.Drop();
            }
        }

        public T Add<T>(T entity, bool generateKey = true) where T : Base
        {
            foreach (var db in DbX)
            {
                if (db is PostgreService && DbX.Count()>1)
                {
                    entity = entity.CloneTo<T>(!generateKey, false);
                }
                entity = db.Add(entity, generateKey);
            }
            return entity;
        }

        public T Get<T>(IEntityKey key) where T : Base
        {
            T result;
            if (typeof(T).GetInterfaces().Contains(typeof(IOneToMany)))
            {
                var entityKey = (EntityKey) key;
                result = Select<T>().First(a => a.Key == entityKey.Key);
            }
            else if (key is AccountKey accKey)
            {
                result = Select<T>().First(a => String.Compare((a as Account).AccountKey, accKey.Account, StringComparison.Ordinal) == 0 
                                                && (a as Account).ProviderKey == accKey.Provider);

            }
            else
            {
                var entityKey = (EntityKey)key;
                result = Select<T>().First(a => a.Key == entityKey.Key);
            }

            return result;
        }


        public void Update<T>(T entity) where T : Base
        {
            foreach (var db in DbX)
            {
                var result = entity switch
                {
                    IOneToMany oneToMany => db.Select<T>()
                        .First(a => a.Key == entity.Key && ((IOneToMany) a).RelatedKey == oneToMany.RelatedKey),
                    Account acc => Select<T>()
                        .First(a => String.Compare((a as Account).AccountKey, acc.AccountKey, StringComparison.Ordinal) == 0 &&
                                    (a as Account).ProviderKey == acc.ProviderKey),
                    _ => db.Select<T>().First(a => a.Key == entity.Key)
                };
                entity.CopyTo(result, false, true);
                db.Update(result);
            }
        }

        public void Delete<T>(T entity) where T : Base
        {
            entity.Status = Const.Status.Removed;
            Update(entity);
        }

        public IQueryable<T> Select<T>(string status = Const.Status.Actual) where T : Base
        {
            return DbX.First().Select<T>(status);
        }
    }
}
