using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
using MongoDB.Bson;

namespace Csud.Crud
{
    public partial interface ICsud
    {
        public IQueryable<TEntity> SelectRelational<TEntity>(string status = Const.Status.Actual) where TEntity: Base, IRelational
        {
            var x = Select<TEntity>(status);
            return x;
        }

        public RelationalAggregated<TEntity,TLinked>  GetRelational<TEntity,TLinked> (int key, string status = Const.Status.Actual, bool recursive=true) where TEntity : Base, IRelational where TLinked : Base
        {
            var content = ListRelational<TEntity, TLinked>(key).First();
            return content;
        }

        public IEnumerable<RelationalAggregated<TEntity,TLinked> > ListRelational<TEntity,TLinked> (int key = 0, string status = Const.Status.Actual, int skip = 0, int take = 0) where TEntity : Base, IRelational where TLinked : Base
        {
            var q = SelectRelational<TEntity>(status);
            if (skip != 0)
                q = q.Skip(skip);
            if (take != 0)
                q = q.Take(take);
            if (key != 0)
                q = q.Where(a => a.Key == key);
            var z = q.ToList().GroupBy(x => x.Key);

            foreach (var x in z)
            {
                var subject = Select<TLinked>(status).First(a => a.Key == x.Key);
                var reletedKeys = x.Select(a => a.RelatedKey);
                var relations = Select<TLinked>(status).Where(a => reletedKeys.Contains(a.Key));
                var reslt = new RelationalAggregated<TEntity,TLinked> () { 
                    Subject = subject, 
                      Relations = relations, 
                      Group =z.First(a=> a.Key==x.Key).First(), 
                      Index=x.ToArray()
                    };
                yield return reslt;
            }
        }

        public void InsertRelational<TEntity,TLinked>(TEntity entity, bool generateKey = true) where TEntity : Base, IRelational where TLinked : Base
        {
            if (entity.RelatedKeys == null || entity.RelatedKeys.Count == 0)
                throw new ArgumentException($"Связанные объекты не найдены");
            foreach (var rkey in entity.RelatedKeys)
            {
                if (Select<TLinked>().Any(a => a.Key == rkey) == false)
                    throw new ArgumentException($"Связанный объект с кодом {rkey} не найден");
            }
            int? rootKey = null;
            foreach (var rkey in entity.RelatedKeys)
            {
                var x = (TEntity) entity.Clone();
                x.RelatedKey = rkey;
                x.Key = rootKey;
                AddEntity(x, rootKey == null);
                rootKey ??= x.Key;
            }
        }

        public void DeleteRelational<TEntity>(int key) where TEntity : Base, IRelational
        {
            if (Select<TEntity>().Any(a => a.Key == key) == false)
                throw new ArgumentException($"Объекты с ключем {key} не найдены");
            var all = Select<TEntity>().Where(a => a.Key == key);
            foreach (var item in all)
            {
                DeleteEntity<TEntity>(item);
            }
        }

        public void CopyRelational<TEntity>(int key, bool keepKey = false) where TEntity : Base, IRelational
        {
            if (Select<TEntity>().Any(a => a.Key == key) == false)
                throw new ArgumentException($"Объекты с ключем {key} не найдены");

            var all = Select<TEntity>().Where(a => a.Key == key);
            int? rootKey = null;
            foreach (var item in all)
            {
                item.ID = null;
                item.Key = rootKey;
                var add = CopyEntity<TEntity>(item,rootKey!=null);
                rootKey ??= add.Key;
            }
        }
    }
}
