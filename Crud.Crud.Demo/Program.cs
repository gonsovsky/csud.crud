using System;
using Microsoft.Extensions.Configuration;

namespace Csud.Crud.Demo
{
    class Program
    {
        static void Main()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);

            var config = builder.Build();
            var postgre = config["Postgre"];
            var mongoHost = config["Mongo:Host"];
            var mongoPort = config["Mongo:Port"];
            var mongoDb = config["Mongo:Db"];

            ICsud csud = ICsud.GetDatabase(postgre, mongoHost, int.Parse(mongoPort), mongoDb);
            var gen = new DatasetGenerator(csud);
            gen.Generate();
            Console.WriteLine("Done");
            Console.ReadKey();
        }

    }
}
