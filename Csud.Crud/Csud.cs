using System.Collections.Generic;
using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Mongo;
using Csud.Crud.Postgre;

namespace Csud.Crud
{
    public class Csud: ICsud
    {
        public List<ICsud> Db = new();

        public Csud(string postgreConStr, string mongoHost, int mongoPort, string mongoDb)
        {
            var postgre = new CsudPostgre(postgreConStr);
            var mongo = new CsudMongo(mongoHost, mongoPort, mongoDb);
            Db.Add(mongo);
            Db.Add(postgre);
        }

        public void Add<T>(T entity) where T : Base
        {
            Db.ForEach(x=>
            {
                entity.Key = null;
                x.Add(entity);
            });
        }

        public IQueryable<T> Q<T>() where T : Base
        {
            throw new System.NotImplementedException();
        }

        public void AddPerson(Person person) => Add(person);
        public void AddAccountProvider(AccountProvider provider) => Add(provider);
        public void AddAccount(Account account) => Add(account);
        public void AddSubject(Subject subject) => Add(subject);
        public void AddContext(Context context) => Add(context);
        public void AddTimeContext(TimeContext timeContext) => Add(timeContext);
        public void AddSegmentContext(TimeContext segmentContext) => Add(segmentContext);
    }
}
