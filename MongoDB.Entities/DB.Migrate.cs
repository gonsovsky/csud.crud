﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MongoDB.Entities
{
    public static partial class DB
    {
        /// <summary>
        /// Discover and run migrations from the same assembly as the specified type.
        /// </summary>
        /// <typeparam name="T">A type that is from the same assembly as the migrations you want to run</typeparam>
        public static async Task MigrateAsync<T>() where T : class
        {
            await MigrateAsync(typeof(T)).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes migration classes that implement the IMigration interface in the correct order to transform the database.
        /// <para>TIP: Write classes with names such as: _001_rename_a_field.cs, _002_delete_a_field.cs, etc. and implement IMigration interface on them. Call this method at the startup of the application in order to run the migrations.</para>
        /// </summary>
        public static async Task MigrateAsync()
        {
            await MigrateAsync(null).ConfigureAwait(false);
        }

        private static async Task MigrateAsync(Type targetType)
        {
            IEnumerable<Assembly> assemblies;

            if (targetType == null)
            {
                var excludes = new[]
                {
                    "Microsoft.",
                    "System.",
                    "MongoDB.",
                    "testhost.",
                    "netstandard",
                    "Newtonsoft.",
                    "mscorlib",
                    "NuGet."
                };

                assemblies = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Where(a =>
                          (!a.IsDynamic && !excludes.Any(n => a.FullName.StartsWith(n))) ||
                          a.FullName.StartsWith("MongoDB.Entities.Tests"));
            }
            else
            {
                assemblies = new[] { targetType.Assembly };
            }

            var types = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetInterfaces().Contains(typeof(IMigration)));

            if (!types.Any())
                throw new InvalidOperationException("Didn't find any classes that implement IMigrate interface.");

            var lastMigNum = (
                await Find<Migration, int>()
                      .Sort(m => m.Number, Order.Descending)
                      .Limit(1)
                      .Project(m => m.Number)
                      .ExecuteAsync()
                      .ConfigureAwait(false))
                .SingleOrDefault();

            var migrations = new SortedDictionary<int, IMigration>();

            foreach (var t in types)
            {
                var success = int.TryParse(t.Name.Split('_')[1], out int migNum);

                if (!success)
                    throw new InvalidOperationException("Failed to parse migration number from the class name. Make sure to name the migration classes like: _001_some_migration_name.cs");

                if (migNum > lastMigNum)
                    migrations.Add(migNum, (IMigration)Activator.CreateInstance(t));
            }

            var sw = new Stopwatch();

            foreach (var migration in migrations)
            {
                sw.Start();
                await migration.Value.UpgradeAsync().ConfigureAwait(false);
                var mig = new Migration
                {
                    Number = migration.Key,
                    Name = migration.Value.GetType().Name,
                    TimeTakenSeconds = sw.Elapsed.TotalSeconds
                };
                await SaveAsync(mig).ConfigureAwait(false);
                sw.Stop();
                sw.Reset();
            }
        }
    }
}
