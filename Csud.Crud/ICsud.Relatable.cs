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
        public IQueryable<T> SelectRelatable<T>(string status = Const.Status.Actual) where T: Base, IRelatable
        {
            var x = Select<T>(status);
            return x;
        }

        public T GetRelatable<T>(int key, string status = Const.Status.Actual, bool recursive=true) where T : Base, IRelatable
        {
            var entity = SelectRelatable<T>(status).First(x => x.Key == key);
            if (recursive)
                entity.RelatedEntities = ExpandRelatable<T>(key);
            return entity;
        }

        private IEnumerable ExpandRelatable<T>(int key, string status = Const.Status.Actual) where T : Base, IRelatable
        {
            var all = SelectRelatable<T>(status)
                .Where(a => a.Key == key);
            foreach (var relatable in all)
                yield return GetRelatable<T>((int)relatable.RelatedKey, status, false);
        }

        public IEnumerable<T> ListRelatable<T>(string status = Const.Status.Actual, int skip = 0, int take = 0) where T : Base, IRelatable
        {
            var q = SelectRelatable<T>(status);
            if (skip != 0)
                q = q.Skip(skip);
            if (take != 0)
                q = q.Take(take);
            foreach (var context in q)
            {
                yield return GetRelatable<T>((int)context.Key, status, false);
            }
        }

        public void InsertRelatable<T>(T entity, bool generateKey = true) where T : Base, IRelatable
        {

        }

        public void DeleteRelatable<T>(T entity) where T : Base, IRelatable
        {
    
        }

        public T CopyRelatable<T>(T entity, bool keepKey = false) where T : Base, IRelatable
        {
            return entity;
        }
    }
}
