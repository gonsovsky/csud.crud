using System;
using System.Collections.Generic;
using System.Linq;
using Csud.Crud.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Csud.Crud.Services
{
    public interface IOneToManyService<TEntity, TModelAdd,TModelEdit, TLinked> 
    where TEntity : Base,IOneToMany where TModelAdd : TEntity, IOneToManyAdd where TModelEdit : TEntity, IOneToManyEdit where TLinked : Base
    {
        public IQueryable<TEntity> Select(string status = Const.Status.Actual);

        public OneToMany<TEntity, TLinked> Get(int key, string status = Const.Status.Actual, bool recursive = true);

        public IEnumerable<OneToMany<TEntity, TLinked>> List(int key = 0, string status = Const.Status.Actual,
            int skip = 0, int take = 0);

        public OneToMany<TEntity, TLinked> Add(TModelAdd entity, bool generateKey = true);

        public void Delete(int key);

        public OneToMany<TEntity, TLinked> Update(TModelEdit editEntity);

        public OneToMany<TEntity, TLinked> Copy(int key, bool keepKey = false);

        public OneToMany<TEntity, TLinked> Include(int key, int relatedKey);

        public OneToMany<TEntity, TLinked> Exclude(int key, int relatedKey);
    }

    public class OneToManyService<TEntity, TModelAdd, TModelEdit, TLinked> : OneToAnyService<TEntity, TLinked>,
        IOneToManyService<TEntity, TModelAdd, TModelEdit, TLinked> 
        where TEntity : Base, IOneToMany where TModelAdd : TEntity, IOneToManyAdd where TModelEdit : TEntity, IOneToManyEdit where TLinked : Base
    {
        public OneToManyService(IEntityService<TEntity> entitySvc, IEntityService<TLinked> linkedSvc) : base(entitySvc, linkedSvc)
        {
        }

        public IQueryable<TEntity> Select(string status = Const.Status.Actual)
        {
            var x = EntitySvc.Select(status);
            return x;
        }

        public OneToMany<TEntity, TLinked> Get(int key, string status = Const.Status.Actual, bool recursive = true) 
        {
            var content = List(key).First();

            return content;
        }

        public IEnumerable<OneToMany<TEntity, TLinked>> List(int key = 0, string status = Const.Status.Actual, int skip = 0, int take = 0)
        {
            var q = Select(status);
            if (skip != 0)
                q = q.Skip(skip);
            if (take != 0)
                q = q.Take(take);
            if (key != 0)
                q = q.Where(a => a.Key == key);
            var group = q.ToList().GroupBy(x => x.Key).ToArray();

            foreach (var x in group)
            {
                var subject = LinkedSvc.Select(status).First(a => a.Key == x.Key);
                var relatedKeys = x.Select(a => a.RelatedKey);
                var relations = LinkedSvc.Select(status).Where(a => relatedKeys.Contains(a.Key));
                var result = new OneToMany<TEntity, TLinked>()
                {
                    Subject = subject,
                    Relations = relations,
                    Group = group.First(a => a.Key == x.Key).First(),
                    Index = x.ToArray()
                };
                yield return result;
            }
        }

        public OneToMany<TEntity, TLinked> Add(TModelAdd entity, bool generateKey = true)
        {
            var linked = entity.CloneTo<TLinked>(false);
            entity.Link(linked);
            LinkedSvc.Add(linked);
            entity.Key = linked.Key;
            entity.ID = linked.ID;
            foreach (var rkey in entity.RelatedKeys)
            {
                var x = entity.CloneTo<TEntity>(false);
                x.ID = null;
                x.Key = linked.Key;
                x.RelatedKey = rkey;
                EntitySvc.Add(x, false);
            }
            return Get(linked.Key);
        }


        public OneToMany<TEntity, TLinked> Update(TModelEdit editEntity)
        {
            throw new NotImplementedException();
            //var entity = editEntity.CloneTo<TEntity>(true);
            //var linked = editEntity.CloneTo<TLinked>(true);
            //var existingEntity = EntitySvc.Look(entity.Key);
            //var existingLinked = LinkedSvc.Look(linked.Key);
            //entity.Key = existingEntity.Key;
            //linked.Key = existingLinked.Key;
            //entity.ID = existingEntity.ID;
            //linked.ID = existingLinked.ID;
            //LinkedSvc.Update(linked);
            //entity.Link(linked);
            //EntitySvc.Update(entity);
            //return entity;
        }

        public void Delete(int key)
        {
            if (Select().Any(a => a.Key == key) == false)
                throw new ArgumentException($"Объекты с ключем {key} не найдены");
            var all = Select().Where(a => a.Key == key);
            foreach (var item in all)
            {
                EntitySvc.Delete(item);
            }
            LinkedSvc.Delete(key);
        }

        public OneToMany<TEntity, TLinked> Copy(int key, bool keepKey = false)
        {
            if (Select().Any(a => a.Key == key) == false)
                throw new ArgumentException($"Объекты с ключем {key} не найдены");

            var linked = LinkedSvc.Look(key);
            var copied = LinkedSvc.Copy(linked);

            var all = Select().Where(a => a.Key == key);
            foreach (var item in all)
            {
                item.Key = copied.Key;
                EntitySvc.Copy(item, true);
            }
            return Get(copied.Key);
        }

        public OneToMany<TEntity, TLinked> Include(int key, int relatedKey)
        {
            if (Select().Any(a => a.Key == key) == false)
                throw new ArgumentException($"Объекты с ключем {key} не найдены");
            if (Select().Any(a => a.Key == key && a.RelatedKey == relatedKey))
                throw new ArgumentException($"Пара {key}-{relatedKey} уже существует");
            var linked = LinkedSvc.Look(key);
            var entity = Activator.CreateInstance<TEntity>();
            entity.Key = key;
            entity.RelatedKey = relatedKey;
            entity.Link(linked);
            LinkedSvc.Update(linked);
            EntitySvc.Add(entity, false);
            return Get(key);
        }

        public OneToMany<TEntity, TLinked> Exclude(int key, int relatedKey)
        {
            if (Select().Any(a => a.Key == key) == false)
                throw new ArgumentException($"Объекты с ключем {key} не найдены");
            if (Select().Any(a => a.Key == key && a.RelatedKey == relatedKey) == false)
                throw new ArgumentException($"Пара {key}-{relatedKey} не найдена");
            if (Select().Count(a => a.Key == key) <= 1)
                throw new ArgumentException($"Нельзя удалить последню пару связи с ключем {key}");
            var entity = EntitySvc.Select().First(x => x.Key == key && x.RelatedKey == relatedKey);
            EntitySvc.Delete(entity);
            return Get(key);
        }
    }
}
