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

            CsudService.StartUp(config);
            _csud = CsudService.Csud;
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
            _csud.Upd(person);

            Console.WriteLine("deleting");
            person = _csud.Person.First();
            _csud.Del(person);

            Console.WriteLine("cloning");
            person = _csud.Person.First();
            _csud.Copy(person);
        }
    }
}
