using System;
using Microsoft.Extensions.Configuration;

namespace Csud.Crud.Demo
{
    internal class Program
    {
        private static void Main()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);

            var config = builder.Build();
            var postgre = config["Postgre"];
            var mongoHost = config["Mongo:Host"];
            var mongoPort = config["Mongo:Port"];
            var mongoDb = config["Mongo:Db"];

            ICsud csud = new Csud(postgre, mongoHost, int.Parse(mongoPort), mongoDb);
            var gen = new DataGenerator(csud);
            gen.Generate();
            Console.WriteLine("Done");
            Console.ReadKey();
        }

    }
}
