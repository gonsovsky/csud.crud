using System.Collections.Generic;
using System.Linq;
using Csud.Crud.Models;
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

        public void AddEntity<T>(T entity) where T : Base
        {
            Db.ForEach(x=>
            {
                entity.Key = null;
                x.AddEntity(entity);
            });
        }

        public IQueryable<T> Q<T>() where T : Base
        {
            throw new System.NotImplementedException();
        }
    }
}
