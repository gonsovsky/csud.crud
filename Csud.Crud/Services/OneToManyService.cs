using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csud.Crud.Models;

namespace Csud.Crud.Services
{
    public interface IOneToManyService<TEntity, TModelAdd, TLinked> where TEntity : Base, IOneToMany where TModelAdd : Base, IOneToMany where TLinked : Base
    {
        public IQueryable<TEntity> Select(string status = Const.Status.Actual);

        public OneToManyAggregated<TEntity, TLinked> Get(int key, string status = Const.Status.Actual, bool recursive = true);

        public IEnumerable<OneToManyAggregated<TEntity, TLinked>> List(int key = 0, string status = Const.Status.Actual,
            int skip = 0, int take = 0);

        public void Add(TEntity entity, bool generateKey = true);

        public void Delete(int key);

        public void Copy(int key, bool keepKey = false);

        public void Include(int key, int relatedKey);
    }

    public class OneToManyService<TEntity, TModelAdd, TLinked> : IOneToManyService<TEntity, TModelAdd, TLinked> where TEntity : Base, IOneToMany where TModelAdd : Base, IOneToMany where TLinked : Base
    {
        protected readonly IEntityService<TEntity> EntitySvc;
        protected readonly IEntityService<TLinked> LinkedSvc;

        protected OneToManyService(IEntityService<TEntity> entitySvc, IEntityService<TLinked> linkedSvc)
        {
            EntitySvc = entitySvc;
            LinkedSvc = linkedSvc;
        }

        public IQueryable<TEntity> Select(string status = Const.Status.Actual)
        {
            var x = EntitySvc.Select(status);
            return x;
        }

        public OneToManyAggregated<TEntity, TLinked> Get(int key, string status = Const.Status.Actual, bool recursive = true) 
        {
            var content = List(key).First();
            return content;
        }

        public IEnumerable<OneToManyAggregated<TEntity, TLinked>> List(int key = 0, string status = Const.Status.Actual, int skip = 0, int take = 0)
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
                var result = new OneToManyAggregated<TEntity, TLinked>()
                {
                    Subject = subject,
                    Relations = relations,
                    Group = group.First(a => a.Key == x.Key).First(),
                    Index = x.ToArray()
                };
                yield return result;
            }
        }
        public void Add(TEntity entity, bool generateKey = true)
        {
            if (entity.RelatedKeys == null || entity.RelatedKeys.Count == 0)
                throw new ArgumentException($"Связанные объекты не найдены");
            foreach (var rkey in entity.RelatedKeys)
            {
                if (Select().Any(a => a.Key == rkey) == false)
                    throw new ArgumentException($"Связанный объект с кодом {rkey} не найден");
            }
            var linked = Activator.CreateInstance<TLinked>();
            entity.CopyTo(linked, false);
            entity.Link(linked);
            LinkedSvc.Add(linked);
            entity.Key = linked.Key;
            entity.ID = linked.ID;
            foreach (var rkey in entity.RelatedKeys)
            {
                var x = (TEntity)entity.Clone();
                x.ID = null;
                x.Key = linked.Key;
                x.RelatedKey = rkey;
                EntitySvc.Add(x, false);
            }
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
            EntitySvc.Delete(Select().First(a => a.Key == key));
        }

        public void Copy(int key, bool keepKey = false)
        {

            if (Select().Any(a => a.Key == key) == false)
                throw new ArgumentException($"Объекты с ключем {key} не найдены");

            var co = this.Select().First(a => a.Key == key);
            co = EntitySvc.Copy(co);

            var all = Select().Where(a => a.Key == key);
            foreach (var item in all)
            {
                item.Key = co.Key;
                EntitySvc.Copy(item, true);
            }
        }

        public void Include(int key, int relatedKey)
        {
            if (Select().Any(a => a.Key == key) == false)
                throw new ArgumentException($"Объекты с ключем {key} не найдены");

            if (Select().Any(a => a.Key == key && a.RelatedKey == relatedKey) == false)
                throw new ArgumentException($"Пара {key}-{relatedKey} не найдена");
        }
    }
}
