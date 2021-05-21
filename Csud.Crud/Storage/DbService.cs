using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csud.Crud.Models;
using Csud.Crud.Models.App;
using Csud.Crud.Models.Maintenance;

namespace Csud.Crud.Storage
{
    public interface IDbService
    {
        public void Add<T>(T entity, bool generateKey = true) where T : Base;

        public void Update<T>(T entity) where T : Base;

        public IQueryable<T> Select<T>(string status = Const.Status.Actual) where T : Base;
    }

    public class DbService: IDbService
    {
        public List<IDbService> Db = new();

        public MongoService Mongo;

        public PostgreService Postgre;
        public DbService(Config cfg)
        {
            if (cfg.Mongo.Enabled)
            {
                Mongo = new MongoService(cfg);
                Db.Add(Mongo);
            }
            if (cfg.Postgre.Enabled)
            {
                Postgre = new PostgreService(cfg);
                Db.Add(Postgre);
            }
        }


        public void Add<T>(T entity, bool generateKey = true) where T : Base
        {
            foreach (var x in Db)
            {
                x.Add(entity, generateKey);
            }
        }

        public void Update<T>(T entity) where T : Base
        {
            Db.ForEach(x =>
            {
                if (x is PostgreService)
                {
                    var y = x.Select<T>().First(a => a.Key == entity.Key);
                    entity.CopyTo(y, false);
                    x.Update(y);
                    return;
                }
                x.Update(entity);
            });
        }

        public IQueryable<T> Select<T>(string status = Const.Status.Actual) where T : Base
        {
            return Db.First().Select<T>(status);
        }

        public IQueryable<App> App => Select<App>();
        public IQueryable<AppDistrib> AppDistrib => Select<AppDistrib>();
        public IQueryable<AppRole> AppRole => Select<AppRole>();
        public IQueryable<AppRoleDefinition> AppRoleDefinition => Select<AppRoleDefinition>();
        public IQueryable<AppRoleDetails> AppRoleDetails => Select<AppRoleDetails>();
        public IQueryable<AppEntityDefinition> AppEntityDefinition => Select<AppEntityDefinition>();
        public IQueryable<AppEntity> AppEntity => Select<AppEntity>();
        public IQueryable<AppAttributeDefinition> AppAttributeDefinition => Select<AppAttributeDefinition>();
        public IQueryable<AppAttribute> AppAttribute => Select<AppAttribute>();
        public IQueryable<AppOperationDefinition> AppOperationDefinition => Select<AppOperationDefinition>();
        public IQueryable<AppOperation> AppOperation => Select<AppOperation>();
        public IQueryable<AppImport> AppImport => Select<AppImport>();
    }
}
