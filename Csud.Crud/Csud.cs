using System.Collections.Generic;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Mongo;
using Csud.Crud.Postgre;

namespace Csud.Crud
{
    public class Csud: ICsud
    {
        protected List<ICsud> Db = new();

        public Csud(string postgreConStr, string mongoHost, int mongoPort, string mongoDb)
        {
            var postgre = new CsudPostgre(postgreConStr);
            var mongo = new CsudMongo(mongoHost, mongoPort, mongoDb);
            Db.Add(mongo);
            Db.Add(postgre);
        }

        public void AddEntity<T>(T entity) where T : Base
        {
            Db.ForEach(x=>
            {
                entity.Key = null;
                x.AddEntity(entity);
            });
        }
        public void AddPerson(Person person) => AddEntity(person);
        public void AddAccountProvider(AccountProvider provider) => AddEntity(provider);
        public void AddAccount(Account account) => AddEntity(account);
        public void AddSubject(Subject subject) => AddEntity(subject);
        public void AddContext(Context context) => AddEntity(context);
        public void AddTimeContext(TimeContext timeContext) => AddEntity(timeContext);
        public void AddSegmentContext(TimeContext segmentContext) => AddEntity(segmentContext);
    }
}
