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
        public IQueryable<T> SelectRelational<T>(string status = Const.Status.Actual) where T: Base, IRelational
        {
            var x = Select<T>(status);
            return x;
        }

        public RelationalAggregated<T,T1> GetRelational<T,T1>(int key, string status = Const.Status.Actual, bool recursive=true) where T : Base, IRelational where T1 : Base
        {
            var content = ListRelational<T, T1>(key).First();
            return content;
        }

        public IEnumerable<RelationalAggregated<T,T1>> ListRelational<T,T1>(int key = 0, string status = Const.Status.Actual, int skip = 0, int take = 0) where T : Base, IRelational where T1: Base
        {
            var q = SelectRelational<T>(status);
            if (skip != 0)
                q = q.Skip(skip);
            if (take != 0)
                q = q.Take(take);
            if (key != 0)
                q = q.Where(a => a.Key == key);
            var z = q.ToList().GroupBy(x => x.Key);

            foreach (var x in z)
            {
                var subject = Select<T1>(status).First(a => a.Key == x.Key);
                var reletedKeys = x.Select(a => a.RelatedKey);
                var relations = Select<T1>(status).Where(a => reletedKeys.Contains(a.Key));
                var reslt = new RelationalAggregated<T,T1>() { 
                    Subject = subject, 
                      Relations = relations, 
                      Group =z.First(a=> a.Key==x.Key).First(), 
                      Index=x.ToArray()
                    };
                yield return reslt;
            }
        }

        public void InsertRelational<TGroup,TEntity>(TGroup entity, bool generateKey = true) where TGroup : Base, IRelational where TEntity : Base
        {
            if (entity.RelatedKeys == null || entity.RelatedKeys.Count == 0)
                throw new ArgumentException($"Связанные объекты не найдены");
            foreach (var rkey in entity.RelatedKeys)
            {
                if (Select<TEntity>().Any(a => a.Key == rkey) == false)
                    throw new ArgumentException($"Связанный объект с кодом {rkey} не найден");
            }
            int? rootKey = null;
            foreach (var rkey in entity.RelatedKeys)
            {
                var x = (TGroup) entity.Clone();
                x.RelatedKey = rkey;
                x.Key = rootKey;
                AddEntity(x, rootKey == null);
                rootKey ??= x.Key;
            }
        }

        public void DeleteRelational<T>(int key) where T : Base, IRelational
        {
            if (Select<T>().Any(a => a.Key == key) == false)
                throw new ArgumentException($"Объекты с ключем {key} не найдены");
            var all = Select<T>().Where(a => a.Key == key);
            foreach (var item in all)
            {
                DeleteEntity<T>(item);
            }
        }

        public void CopyRelational<T>(int key, bool keepKey = false) where T : Base, IRelational
        {
            if (Select<T>().Any(a => a.Key == key) == false)
                throw new ArgumentException($"Объекты с ключем {key} не найдены");

            var all = Select<T>().Where(a => a.Key == key);
            int? rootKey = null;
            foreach (var item in all)
            {
                item.ID = null;
                item.Key = rootKey;
                var add = CopyEntity<T>(item,rootKey!=null);
                rootKey ??= add.Key;
            }
        }
    }
}
