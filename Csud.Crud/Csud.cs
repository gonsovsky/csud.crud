﻿using System.Collections.Generic;
using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Mongo;
using Csud.Crud.Postgre;

namespace Csud.Crud
{
    public class Csud: ICsud
    {
        public List<ICsud> Db = new();

        public CsudMongo Mongo;

        public CsudPostgre Postgre;

        public Csud(string postgreConStr, string mongoHost, int mongoPort, string mongoDb)
        {
            Postgre = new CsudPostgre(postgreConStr);
            Mongo = new CsudMongo(mongoHost, mongoPort, mongoDb);
            Db.Add(Mongo);
            Db.Add(Postgre);
        }

        public void AddEntity<T>(T entity, bool keyDefined = false) where T : Base
        {
            foreach (var x in Db)
            {
                if (keyDefined == false)
                    entity.Key = null;
                x.AddEntity(entity);
            }
        }

        public void UpdateEntity<T>(T entity) where T : Base
        {
            Db.ForEach(x =>
            {
                if (x is CsudPostgre)
                {
                    var y = x.Q<T>().First(a => a.Key == entity.Key);
                    entity.CopyTo(y, false);
                    x.UpdateEntity(y);
                    return;
                }
                x.UpdateEntity(entity);
            });
        }

        public IQueryable<T> Q<T>() where T : Base
        {
            return Db.First().Q<T>();
        }
    }
}
