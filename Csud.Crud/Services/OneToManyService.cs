using System;
using System.Collections.Generic;
using System.Linq;
using Csud.Crud.Models;

namespace Csud.Crud.Services
{
    public interface IOneToManyService<TEntity, TModelAdd,TModelEdit, TLinked> 
    where TEntity : Base,IOneToMany where TModelAdd : TEntity, IOneToManyAdd where TModelEdit : TEntity, IOneToManyEdit where TLinked : Base
    {
        public IQueryable<TEntity> Select(string status = Const.Status.Actual);

        public IOneToManyRecord<TEntity, TLinked> Get(int key, string status = Const.Status.Actual, bool recursive = true);

        public IEnumerable<IOneToManyRecord<TEntity, TLinked>> List(int key = 0, string status = Const.Status.Actual,
            int skip = 0, int take = 0);

        public IOneToManyRecord<TEntity, TLinked> Add(TModelAdd entity, bool generateKey = true);

        public void Delete(int key);

        public IOneToManyRecord<TEntity, TLinked> Copy(int key, bool keepKey = false);

        public IOneToManyRecord<TEntity, TLinked> Include(int key, int relatedKey);

        public IOneToManyRecord<TEntity, TLinked> Exclude(int key, int relatedKey);
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

        public IOneToManyRecord<TEntity, TLinked> Get(int key, string status = Const.Status.Actual, bool recursive = true) 
        {
            var content = List(key).First();
            return content;
        }

        public IEnumerable<IOneToManyRecord<TEntity, TLinked>> List(int key = 0, string status = Const.Status.Actual, int skip = 0, int take = 0)
        {
            var q = Select(status);
            if (skip != 0)
                q = q.Skip(skip);
            if (take != 0)
                q = q.Take(take);
            if (key != 0)
                q = q.Where(a => a.Key == key);
            var groups = q.ToList().GroupBy(x => x.Key).ToArray();

            foreach (var groupped in groups)
            {
                var group = groupped.ToArray();
                var keys = group.Select(b => b.RelatedKey).ToList();

                var link = LinkedSvc.Select(status)
                    .First(a => a.Key == groupped.Key);

                var links = LinkedSvc.Select(status)
                    .Where(a => keys.Contains(a.Key)).ToArray();

                var relations = group.Join(links,
                    p => p.RelatedKey,
                    t => t.Key,
                    (p, t) => group[0].MakeOneToManyItem(p, t)).ToArray();


                var result = group[0].MakeOneToManyRecord(link, relations);
         
                yield return result;
            }
        }

        public IOneToManyRecord<TEntity, TLinked> Add(TModelAdd entity, bool generateKey = true)
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

        public IOneToManyRecord<TEntity, TLinked> Copy(int key, bool keepKey = false)
        {
            if (Select().Any(a => a.Key == key) == false)
                throw new ArgumentException($"Объекта с ключем {key} не найдено");

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

        public IOneToManyRecord<TEntity, TLinked> Include(int key, int relatedKey)
        {
            if (LinkedSvc.Select().Any(a => a.Key == key) == false)
                throw new ArgumentException($"Объекта с ключем {key} не найдено");
            if (Select(Const.Status.Actual).Any(a => a.Key == key && a.RelatedKey == relatedKey))
                throw new ArgumentException($"Пара {key}-{relatedKey} уже существует");

            if (Select(Const.Status.Any).Any(a => a.Key == key && a.RelatedKey == relatedKey))
            {
                var entity = Select(Const.Status.Any).First(a => a.Key == key && a.RelatedKey == relatedKey);
                EntitySvc.Restore(entity);
            }
            else
            {
                var entity = Activator.CreateInstance<TEntity>();
                entity.Key = key;
                entity.RelatedKey = relatedKey;
                EntitySvc.Add(entity, false);
            }
            return Get(key);
        }

        public IOneToManyRecord<TEntity, TLinked> Exclude(int key, int relatedKey)
        {
            if (LinkedSvc.Select().Any(a => a.Key == key) == false)
                throw new ArgumentException($"Объекта с ключем {key} не найдено");
            if (Select().Any(a => a.Key == key && a.RelatedKey == relatedKey) == false)
                throw new ArgumentException($"Пары {key}-{relatedKey} не найдено");
            if (Select().Count(a => a.Key == key) <= 1)
                throw new ArgumentException($"Нельзя удалить последню пару связи с ключем {key}");
            var entity = EntitySvc.Select().First(x => x.Key == key && x.RelatedKey == relatedKey);
            EntitySvc.Delete(entity);
            return Get(key);
        }
    }
}
