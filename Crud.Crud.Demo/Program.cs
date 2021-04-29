using System;
using System.Linq;
using Csud.Crud.Models;
using Microsoft.Extensions.Configuration;

namespace Csud.Crud.Demo
{
    internal class Program
    {
        private static Csud csud;
        private static void Main()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);

            var config = builder.Build();
            var postgre = config["Postgre"];
            var mongoHost = config["Mongo:Host"];
            var mongoPort = config["Mongo:Port"];
            var mongoDb = config["Mongo:Db"];

            csud = new Csud(postgre, mongoHost, int.Parse(mongoPort), mongoDb);
            var gen = new DataGenerator(csud);
            gen.Generate();
            Test();
            Console.WriteLine("Done");
            Console.ReadKey();
        }

        private static void Test()
        {
            Console.WriteLine("persons");
            csud.Db.ForEach((db) =>
            {
                Console.WriteLine(db.Person.Count());
            });
            Console.WriteLine("accounts");
            csud.Db.ForEach((db) =>
            {
                Console.WriteLine(db.Account.Count());
            });
            Console.WriteLine("contexts");
            csud.Db.ForEach((db) =>
            {
                Console.WriteLine(db.Context.Count());
            });
            //csud.Db.ForEach((db) =>
            //{
            //    var q = from s in db.Subject
            //        join a in db.Account on s.Key equals a.SubjectKey
            //        join ap in db.AccountProvider on a.AccountProviderKey equals ap.Key
            //        join p in db.Person on a.PersonKey equals p.Key
            //        join co in db.Context on s.ContextKey equals co.Key
            //        join tc in db.TimeContext on co.Key equals tc.ContextKey
            //        join o in db.Object on co.Key equals o.ContextKey
            //        join t in db.Task on o.Key equals t.ObjectKey
            //        select new {p.DisplayName};
            //    foreach (var item in q)
            //    {
            //        Console.WriteLine(item.DisplayName);
            //    }
            //});
        }
    }
}
