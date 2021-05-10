using System;
using System.Collections;
using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Models.Rules;

namespace Csud.Crud
{
    public partial interface ICsud
    {
        public void Insert<T>(T entity, bool generateKey = true) where T : Base;
        public void Upd<T>(T entity) where T : Base;
        public void Del<T>(T entity) where T : Base
        {
            entity.Status = Const.Status.Removed;
            Upd(entity);
        }
        public T Copy<T>(T entity, bool keepKey=false) where T : Base
        {
            var a = (T) entity.Clone(keepKey);
            Insert(a, !keepKey);
            return a;
        }
        public void Restore<T>(T entity) where T : Base
        {
            entity.Status = Const.Status.Actual;
            Upd(entity);
        }
        public IQueryable<T> Select<T>(string status = Const.Status.Actual) where T : Base;
        public IQueryable<T> List<T>(string status = Const.Status.Actual, int skip = 0, int take = 0) where T : Base
        {
            var q = Select<T>(status);
            if (skip != 0)
                q = q.Skip(skip);
            if (take != 0)
                q = q.Take(take);
            return q;
        }
  
        public IQueryable<Person> Person => Select<Person>();
        public IQueryable<AccountProvider> AccountProvider => Select<AccountProvider>();
        public IQueryable<Account> Account => Select<Account>();
        public IQueryable<Subject> Subject => Select<Subject>();
        public IQueryable<ObjectX> Object => Select<ObjectX>();
        public IQueryable<TaskX> Task => Select<TaskX>();
    }
}
