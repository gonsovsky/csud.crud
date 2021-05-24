using System.Collections.Generic;
using System.Linq;
using Csud.Crud.Models;

namespace Csud.Crud.Services
{
    public interface IOneToOneService<TEntity, in TModelAdd, in TModelEdit, TLinked>: IOneToAnyService<TEntity, TLinked>
        where TEntity : Base, IOneToOne where TModelAdd : Base where TModelEdit : Base where TLinked : Base
    {
        public IEnumerable<TEntity> Select();
        public IEnumerable<TEntity> List(string status = Const.Status.Actual, int skip = 0, int take = 0);
        public TEntity Get(int key);
        public TEntity Add(TModelAdd addEntity, bool generateKey = true);
        public TEntity Update(TModelEdit editEntity);
        public void Delete(int key);
        public void Delete(TEntity t);
        public TEntity Copy(int key, bool keepKey = false);
    }

    public class OneToOneService<TEntity, TModelAdd, TModelEdit, TLinked> : OneToAnyService<TEntity, TLinked>, 
        IOneToOneService<TEntity, TModelAdd, TModelEdit, TLinked>
        where TEntity : Base, IOneToOne where TModelAdd : Base where TModelEdit : Base where TLinked : Base
    {
        public OneToOneService(IEntityService<TEntity> entitySvc, IEntityService<TLinked> linkedSvc) : base(entitySvc, linkedSvc)
        {
        }

        public IEnumerable<TEntity> Select()
        {
            foreach (var entity in EntitySvc.Select())
                yield return (TEntity) entity.Combine(LinkedSvc.Look(entity.Key),true);
        }

        public TEntity Get(int key)
        {
            return Select().First(a => a.Key == key);
        }

        public IEnumerable<TEntity> List(string status = Const.Status.Actual, int skip = 0, int take = 0)
        {
            var q = Select();
            if (status != "")
                q = q.Where(a => a.Status == status);
            if (skip != 0)
                q = q.Skip(skip);
            if (take != 0)
                q = q.Take(take);
            return q;
        }

        public TEntity Add(TModelAdd addEntity, bool generateKey = true)
        {
            var entity = addEntity.CloneTo<TEntity>(false, false);
            var linked = addEntity.CloneTo<TLinked>(false, false);
            var result = LinkedSvc.Add(linked);
            entity.Link(result);
            entity.Key = result.Key;
            EntitySvc.Add(entity, false);
            return entity;
        }
        public TEntity Update(TModelEdit editEntity)
        {
            var entity = editEntity.CloneTo<TEntity>(true,true);
            var linked = editEntity.CloneTo<TLinked>(true, true);
            var existingEntity = EntitySvc.Look(entity.Key);
            var existingLinked = LinkedSvc.Look(linked.Key);
            entity.Key = existingEntity.Key;
            linked.Key = existingLinked.Key;
            entity.ID = existingEntity.ID;
            linked.ID = existingLinked.ID;
            LinkedSvc.Update(linked);
            entity.Link(linked);
            EntitySvc.Update(entity);
            return entity;
        }

        public void Delete(int key)
        {
            EntitySvc.Delete(key);
            LinkedSvc.Delete(key);
        }

        public void Delete(TEntity t)
        {
            LinkedSvc.Delete(t.Key);
            EntitySvc.Delete(t);
        }

        public TEntity Copy(int key, bool keepKey = false)
        {
            var linked = LinkedSvc.Look(key);
            var entity = EntitySvc.Look(key);
            var copied = LinkedSvc.Copy(linked);
            entity.Key = copied.Key;
            entity = EntitySvc.Copy(entity, true);
            return entity;
        }
    }
}
