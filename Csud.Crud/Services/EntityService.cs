using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Models.Internal;
using Csud.Crud.Storage;

namespace Csud.Crud.Services
{
    public interface IEntityService<T> where T : Base
    {
        public T Add(T addEntity, bool generateKey = true);
        public T Update(T entity);
        public void Delete(T entity);
        public void Delete(int key);
        public T Copy(T entity, bool keepKey = false);
        public void Restore(T entity);
        public IQueryable<T> Select(string status = Const.Status.Actual);
        public IQueryable<T> List(string status = Const.Status.Actual, int skip = 0, int take = 0);
        public T Look(int key);
        public T Get(IEntityKey key);
    }

    public class EntityService<T> : IEntityService<T> where T : Base
    {
        public IDbService Db;

        public EntityService(IDbService dbSvc)
        {
            Db = dbSvc;
        }

        public T Look(int key)
        {
            return Select().First(x => x.Key == key);
        }

        public T Get(IEntityKey key)
        {
            throw new System.NotImplementedException();
        }

        public virtual T Add(T addEntity, bool generateKey = true)
        {
            var entity = addEntity.CloneTo<T>(!generateKey, false);
            Db.Add(entity, generateKey);
            return entity;
        }

        public T Update(T entity)
        {
            Db.Update(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            entity.Status = Const.Status.Removed;
            Update(entity);
        }

        public void Delete(int key)
        {
            var entity = Db.Select<T>().First(x => x.Key == key);
            Delete(entity);
        }

        public T Copy(T entity, bool keepKey = false)
        {
            var a = (T)entity.Clone(keepKey,false);
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
            return Db.Select<T>(status);
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
