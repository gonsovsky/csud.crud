using System;
using System.Linq;
using Csud.Crud.Models.Rules;
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

            Test1();
            Console.WriteLine("Done");
            Console.ReadKey();
        }

        private static void Test1()
        {
            var n = 0;
            var g = CsudService.CsudObj.Mongo.Select<Group>().ToList().GroupBy(x => x.Key);
            foreach (var x in g)
            {
                Console.WriteLine($"{x.Key} - {x.Count()}");
                foreach (var y in x)
                {
                    y.Dump();
                    Console.WriteLine();
                    n++;
                    if (n > 20)
                        break;
                }
            }
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
            _csud.DeleteEntity(person);

            Console.WriteLine("cloning");
            person = _csud.Person.First();
            _csud.CopyEntity(person);
        }
    }
}
