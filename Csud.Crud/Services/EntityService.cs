using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csud.Crud.Models;
using Csud.Crud.Mongo;
using Csud.Crud.Postgre;

namespace Csud.Crud.Services
{
    public interface IEntityService<T> where T : Base
    {
        public T Add(T entity, bool generateKey = true);
        public T Update(T entity);
        public void Delete(T entity);
        public void Delete(int key);
        public T Copy(T entity, bool keepKey = false);
        public void Restore(T entity);
        public IQueryable<T> Select(string status = Const.Status.Actual);
        public IQueryable<T> List(string status = Const.Status.Actual, int skip = 0, int take = 0);

        public T Look(int key);
    }

    public class EntityService<T> : IEntityService<T> where T : Base
    {
        public List<ICsud> Db = new();

        public CsudMongo Mongo;

        public CsudPostgre Postgre;

        public EntityService()
        {
            var cfg = CsudService.Config;
            if (cfg.Mongo.Enabled)
            {
                Mongo = new CsudMongo(cfg);
                Db.Add(Mongo);
            }
            if (cfg.Postgre.Enabled)
            {
                Postgre = new CsudPostgre(cfg);
                Db.Add(Postgre);
            }
        }

        public T Look(int key)
        {
            return Select().First(x => x.Key == key);
        }

        public T Add(T entity, bool generateKey = true)
        {
            foreach (var x in Db)
            {
                if (x is CsudPostgre)
                    entity = (T) entity.Clone(!generateKey);
                x.AddEntity(entity, generateKey);
            }

            return entity;
        }

        public T Update(T entity)
        {
            Db.ForEach(x =>
            {
                if (x is CsudPostgre)
                {
                    var y = x.Select<T>().First(a => a.Key == entity.Key);
                    entity.CopyTo(y, false);
                    x.UpdateEntity(y);
                    return;
                }
                x.UpdateEntity(entity);
            });
            return entity;
        }

        public void Delete(T entity)
        {
            entity.Status = Const.Status.Removed;
            Update(entity);
        }

        public void Delete(int key)
        {
            var entity = Db.First().Select<T>().First(x => x.Key == key);
            Delete(entity);
        }

        public T Copy(T entity, bool keepKey = false)
        {
            var a = (T)entity.Clone(keepKey);
            Add(a, !keepKey);
            return a;
        }

        public void Restore(T entity)
        {
            entity.Status = Const.Status.Actual;
            Update(entity);
        }

        public IQueryable<T> Select(string status = Const.Status.Actual)
        {
            return Db.First().Select<T>(status);
        }

        public IQueryable<T> List(string status = Const.Status.Actual, int skip = 0, int take = 0)
        {
            var q = Select(status);
            if (skip != 0)
                q = q.Skip(skip);
            if (take != 0)
                q = q.Take(take);
            return q;
        }
    }
}
