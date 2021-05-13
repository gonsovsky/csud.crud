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
        public Csud(Config cfg)
        {
            if (cfg.Mongo.Enabled)
            {
                Mongo = new CsudMongo(cfg);
                Db.Add(Mongo);
            }
            if (cfg.Postgre.Enabled)
            {
                Postgre = new CsudPostgre(cfg);
                Db.Add(Postgre);
            }
        }

        public void AddEntity<T>(T entity, bool generateKey = true) where T : Base
        {
            foreach (var x in Db)
            {
                x.AddEntity(entity, generateKey);
            }
        }

        public void UpdateEntity<T>(T entity) where T : Base
        {
            Db.ForEach(x =>
            {
                if (x is CsudPostgre)
                {
                    var y = x.Select<T>().First(a => a.Key==entity.Key);
                    entity.CopyTo(y, false);
                    x.UpdateEntity(y);
                    return;
                }
                x.UpdateEntity(entity);
            });
        }
        public IQueryable<T> Select<T>(string status = Const.Status.Actual) where T : Base
        {
            return Db.First().Select<T>(status);
        }
    }
}
