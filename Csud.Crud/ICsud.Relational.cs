using System;
using System.Collections.Generic;
using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Models.Rules;
using Csud.Crud.Services;

namespace Csud.Crud
{
    public partial interface ICsud
    {
        public IQueryable<TEntity> SelectRelational<TEntity>(string status = Const.Status.Actual) where TEntity: Base, IOneToMany
        {
            var x = Select<TEntity>(status);
            return x;
        }
        public OneToManyAggregated<TEntity,TLinked>  GetRelational<TEntity,TLinked> (int key, string status = Const.Status.Actual, bool recursive=true) where TEntity : Base, IOneToMany where TLinked : Base
        {
            var content = ListRelational<TEntity, TLinked>(key).First();
            return content;
        }
        public IEnumerable<OneToManyAggregated<TEntity,TLinked>> ListRelational<TEntity,TLinked> (int key = 0, string status = Const.Status.Actual, int skip = 0, int take = 0) where TEntity : Base, IOneToMany where TLinked : Base
        {
            var q = SelectRelational<TEntity>(status);
            if (skip != 0)
                q = q.Skip(skip);
            if (take != 0)
                q = q.Take(take);
            if (key != 0)
                q = q.Where(a => a.Key == key);
            var group = q.ToList().GroupBy(x => x.Key).ToArray();

            foreach (var x in group)
            {
                var subject = Select<TLinked>(status).First(a => a.Key == x.Key);
                var relatedKeys = x.Select(a => a.RelatedKey);
                var relations = Select<TLinked>(status).Where(a => relatedKeys.Contains(a.Key));
                var result = new OneToManyAggregated<TEntity,TLinked> () { 
                    Subject = subject, 
                      Relations = relations, 
                      Group = group.First(a=> a.Key==x.Key).First(), 
                      Index=x.ToArray()
                    };
                yield return result;
            }
        }
        public void AddRelational<TEntity,TLinked>(TEntity entity, bool generateKey = true) where TEntity : Base, IOneToMany where TLinked : Base
        {
            if (entity.RelatedKeys == null || entity.RelatedKeys.Count == 0)
                throw new ArgumentException($"Связанные объекты не найдены");
            foreach (var rkey in entity.RelatedKeys)
            {
                if (Select<TLinked>().Any(a => a.Key == rkey) == false)
                    throw new ArgumentException($"Связанный объект с кодом {rkey} не найден");
            }
            var linked = Activator.CreateInstance<TLinked>();
            entity.CopyTo(linked, false);
            entity.Link(linked);
            AddEntity(linked);
            entity.Key = linked.Key;
            entity.ID = linked.ID;
            foreach (var rkey in entity.RelatedKeys)
            {
                var x = (TEntity) entity.Clone();
                x.ID = null;
                x.Key = linked.Key;
                x.RelatedKey = rkey;
                AddEntity(x, false);
            }
        }
        public void DeleteRelational<TEntity, TLinked>(int key) where TEntity : Base, IOneToMany where TLinked : Base
        {
            if (Select<TEntity>().Any(a => a.Key == key) == false)
                throw new ArgumentException($"Объекты с ключем {key} не найдены");
            var all = Select<TEntity>().Where(a => a.Key == key);
            foreach (var item in all)
            {
                DeleteEntity(item);
            }
            DeleteEntity(Select<TLinked>().First(a=> a.Key==key));
        }
        public void CopyRelational<TEntity, TLinked>(int key, bool keepKey = false) where TEntity : Base, IOneToMany where TLinked : Base
        {
            if (Select<TEntity>().Any(a => a.Key == key) == false)
                throw new ArgumentException($"Объекты с ключем {key} не найдены");

            var co = this.Select<TLinked>().First(a => a.Key == key);
            co = CopyEntity(co);

            var all = Select<TEntity>().Where(a => a.Key == key);
            foreach (var item in all)
            {
                item.Key = co.Key;
                CopyEntity(item, true);
            }
        }
        public void IncludeRelational<TEntity>(int key, int relatedKey) where TEntity : Base, IOneToMany
        {
            if (Select<TEntity>().Any(a => a.Key == key) == false)
                throw new ArgumentException($"Объекты с ключем {key} не найдены");

            if (Select<TEntity>().Any(a => a.Key == key && a.RelatedKey== relatedKey) == false)
                throw new ArgumentException($"Пара {key}-{relatedKey} не найдена");
        }
    }
}
