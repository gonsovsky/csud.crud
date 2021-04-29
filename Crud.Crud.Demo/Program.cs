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
                Console.WriteLine(db.Q<Person>().Count());
            });
            Console.WriteLine("accounts");
            csud.Db.ForEach((db) =>
            {
                Console.WriteLine(db.Q<Account>().Count());
            });
            Console.WriteLine("contexts");
            csud.Db.ForEach((db) =>
            {
                Console.WriteLine(db.Q<Account>().Count());
            });
        }

    }
}
