using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Csud.Crud.Demo
{
    internal class Program
    {
        private static ICsud _csud;
        private static void Main()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);

            var config = builder.Build();
            var postgre = config["Postgre"];
            var mongoHost = config["Mongo:Host"];
            var mongoPort = config["Mongo:Port"];
            var mongoDb = config["Mongo:Db"];

            _csud = new Csud(postgre, mongoHost, int.Parse(mongoPort), mongoDb);
            var gen = new DataGenerator(_csud);
            gen.Generate(100);

            Test();
            Console.WriteLine("Done");
            Console.ReadKey();
        }

        private static void Test()
        {
            Console.WriteLine("getting");
            var person = _csud.Person.First();
            var q = _csud.Person.ToList();
            person = q.First(x => x.Key == 1);

            Console.WriteLine("updating");
            person = _csud.Person.First();
            person.FirstName = "Updated";
            _csud.UpdateEntity(person);

            Console.WriteLine("deleting");
            person = _csud.Person.First();
            _csud.DelEntity(person);

            Console.WriteLine("cloning");
            person = _csud.Person.First();
            _csud.CopyEntity(person);
        }
    }
}
