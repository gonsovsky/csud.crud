using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csud.Crud.Models.App;
using MongoDB.Entities;

namespace Csud.Crud.Mongo
{
    public partial class CsudMongo
    {
        public void AppCsudMongo(Config cfg)
        {
            DB.Index<App>().Option(o => { o.Background = false; o.Unique = true; })
                .Key(a => a.AppKey, KeyType.Text)
                .CreateAsync().Wait();

            DB.Index<AppDistrib>().Option(o => { o.Background = false; o.Unique = true; })
                .Key(a => a.AppKey, KeyType.Text)
                .Key(a => a.DistribKey, KeyType.Text)
                .CreateAsync().Wait();

            DB.Index<AppRole>().Option(o => { o.Background = false; o.Unique = false; })
                .Key(a => a.DistribKey, KeyType.Text)
                .Key(a => a.RoleName, KeyType.Text)
                .CreateAsync().Wait();

            DB.Index<AppRoleDefinition>().Option(o => { o.Background = false; o.Unique = true; })
                .Key(a => a.RoleKey, KeyType.Text)
                .CreateAsync().Wait();

            DB.Index<AppEntityDefinition>().Option(o => { o.Background = false; o.Unique = true; })
                .Key(a => a.EntityKey, KeyType.Text)
                .CreateAsync().Wait();


            DB.Index<AppEntity>().Option(o => { o.Background = false; o.Unique = false; })
                .Key(a => a.DistribKey, KeyType.Text)
                .Key(a => a.EntityName, KeyType.Text)
                .CreateAsync().Wait();
        }
    }
}
