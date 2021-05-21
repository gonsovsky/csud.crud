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

            DB.Index<AppRoleDetails>().Option(o => { o.Background = false; o.Unique = false; })
                .Key(a => a.RoleKey, KeyType.Text)
                .Key(a => a.OperationKey, KeyType.Text)
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

            DB.Index<AppAttributeDefinition>().Option(o => { o.Background = false; o.Unique = true; })
                .Key(a => a.AttributeKey, KeyType.Text)
                .CreateAsync().Wait();

            DB.Index<AppAttribute>().Option(o => { o.Background = false; o.Unique = false; })
                .Key(a => a.DistribKey, KeyType.Text)
                .Key(a => a.AttributeType, KeyType.Text)
                .CreateAsync().Wait();

            DB.Index<AppOperationDefinition>().Option(o => { o.Background = false; o.Unique = true; })
                .Key(a => a.OperationKey, KeyType.Text)
                .CreateAsync().Wait();

            DB.Index<AppOperation>().Option(o => { o.Background = false; o.Unique = false; })
                .Key(a => a.DistribKey, KeyType.Text)
                .Key(a => a.OperationName, KeyType.Text)
                .CreateAsync().Wait();
        }
    }
}
