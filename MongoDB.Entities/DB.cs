﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MongoDB.Entities
{
    /// <summary>
    /// The main entrypoint for all data access methods of the library
    /// </summary>
    public static partial class DB
    {
        static DB()
        {
            BsonSerializer.RegisterSerializer(new DateSerializer());
            BsonSerializer.RegisterSerializer(new FuzzyStringSerializer());
            BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
            BsonSerializer.RegisterSerializer(typeof(decimal?), new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));

            ConventionRegistry.Register(
                "DefaultConventions",
                new ConventionPack
                {
                        new IgnoreExtraElementsConvention(true),
                        new IgnoreManyPropertiesConvention()
                },
                _ => true);
        }

        internal static event Action DefaultDbChanged;

        private static readonly ConcurrentDictionary<string, IMongoDatabase> dbs = new ConcurrentDictionary<string, IMongoDatabase>();
        private static IMongoDatabase defaultDb;

        /// <summary>
        /// Initializes a MongoDB connection with the given connection parameters.
        /// You can call this method as many times as you want (such as in serverless functions) with the same parameters and the connections won't get duplicated.
        /// </summary>
        /// <param name="database">Name of the database</param>
        /// <param name="host">Address of the MongoDB server</param>
        /// <param name="port">Port number of the server</param>
        public static Task InitAsync(string database, string host = "127.0.0.1", int port = 27017)
        {
            return Initialize(
                new MongoClientSettings { Server = new MongoServerAddress(host, port) }, database);
        }

        /// <summary>
        /// Initializes a MongoDB connection with the given connection parameters.
        /// You can call this method as many times as you want (such as in serverless functions) with the same parameters and the connections won't get duplicated.
        /// </summary>
        /// <param name="database">Name of the database</param>
        /// <param name="settings">A MongoClientSettings object</param>
        public static Task InitAsync(string database, MongoClientSettings settings)
        {
            return Initialize(settings, database);
        }

        private static async Task Initialize(MongoClientSettings settings, string dbName)
        {
            if (string.IsNullOrEmpty(dbName))
                throw new ArgumentNullException(nameof(dbName), "Database name cannot be empty!");

            if (dbs.ContainsKey(dbName))
                return;

            try
            {
                var db = new MongoClient(settings).GetDatabase(dbName);

                if (dbs.Count == 0)
                    defaultDb = db;

                if (dbs.TryAdd(dbName, db))
                    await db.RunCommandAsync((Command<BsonDocument>)"{ping:1}").ConfigureAwait(false);
            }
            catch (Exception)
            {
                dbs.TryRemove(dbName, out _);
                throw;
            }
        }

        /// <summary>
        /// Specifies the database that a given entity type should be stored in. 
        /// Only needed for entity types you want stored in a db other than the default db.
        /// </summary>
        /// <typeparam name="T">Any class that implements IEntity</typeparam>
        /// <param name="database">The name of the database</param>
        public static void DatabaseFor<T>(string database) where T : IEntity
            => TypeMap.AddDatabaseMapping(typeof(T), Database(database));

        /// <summary>
        /// Gets the IMongoDatabase for the given entity type
        /// </summary>
        /// <typeparam name="T">The type of entity</typeparam>
        public static IMongoDatabase Database<T>() where T : IEntity
        {
            return Cache<T>.Database;
        }

        /// <summary>
        /// Gets the IMongoDatabase for a given database name if it has been previously initialized.
        /// You can also get the default database by passing 'default' or 'null' for the name parameter.
        /// </summary>
        /// <param name="name">The name of the database to retrieve</param>
        public static IMongoDatabase Database(string name)
        {
            IMongoDatabase db = null;

            if (dbs.Count > 0)
            {
                if (string.IsNullOrEmpty(name))
                    db = defaultDb;
                else
                    dbs.TryGetValue(name, out db);
            }

            if (db == null)
                throw new InvalidOperationException($"Database connection is not initialized for [{(string.IsNullOrEmpty(name) ? "Default" : name)}]");

            return db;
        }

        /// <summary>
        /// Gets the name of the database a given entity type is attached to. Returns name of default database if not specifically attached.
        /// </summary>
        /// <typeparam name="T">Any class that implements IEntity</typeparam>
        public static string DatabaseName<T>() where T : IEntity
        {
            return Cache<T>.DBName;
        }

        /// <summary>
        /// Switches the default database at runtime
        /// <para>WARNING: Use at your own risk!!! Might result in entities getting saved in the wrong databases under high concurrency situations.</para>
        /// <para>TIP: Make sure to cancel any watchers (change-streams) before switching the default database.</para>
        /// </summary>
        /// <param name="name">The name of the database to mark as the new default database</param>
        public static void ChangeDefaultDatabase(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name), "Database name cannot be null or empty");

            defaultDb = Database(name);

            TypeMap.Clear();

            DefaultDbChanged?.Invoke();
        }

        /// <summary>
        /// Exposes the mongodb Filter Definition Builder for a given type.
        /// </summary>
        /// <typeparam name="T">Any class that implements IEntity</typeparam>
        public static FilterDefinitionBuilder<T> Filter<T>() where T : IEntity
        {
            return Builders<T>.Filter;
        }

        /// <summary>
        /// Exposes the mongodb Sort Definition Builder for a given type.
        /// </summary>
        /// <typeparam name="T">Any class that implements IEntity</typeparam>
        public static SortDefinitionBuilder<T> Sort<T>() where T : IEntity
        {
            return Builders<T>.Sort;
        }

        /// <summary>
        /// Exposes the mongodb Projection Definition Builder for a given type.
        /// </summary>
        /// <typeparam name="T">Any class that implements IEntity</typeparam>
        public static ProjectionDefinitionBuilder<T> Projection<T>() where T : IEntity
        {
            return Builders<T>.Projection;
        }

        /// <summary>
        /// Returns a new instance of the supplied IEntity type
        /// </summary>
        /// <typeparam name="T">Any class that implements IEntity</typeparam>
        public static T Entity<T>() where T : IEntity, new()
        {
            return new T();
        }

        /// <summary>
        /// Returns a new instance of the supplied IEntity type with the ID set to the supplied value
        /// </summary>
        /// <typeparam name="T">Any class that implements IEntity</typeparam>
        /// <param name="ID">The ID to set on the returned instance</param>
        public static T Entity<T>(string ID) where T : IEntity, new()
        {
            return new T { ID = ID };
        }
    }

    internal static class TypeMap
    {
        private static readonly ConcurrentDictionary<Type, IMongoDatabase> TypeToDBMap = new ConcurrentDictionary<Type, IMongoDatabase>();
        private static readonly ConcurrentDictionary<Type, string> TypeToCollMap = new ConcurrentDictionary<Type, string>();

        internal static void AddCollectionMapping(Type entityType, string collectionName)
            => TypeToCollMap[entityType] = collectionName;

        internal static string GetCollectionName(Type entityType)
        {
            TypeToCollMap.TryGetValue(entityType, out string name);
            return name;
        }

        internal static void AddDatabaseMapping(Type entityType, IMongoDatabase database)
            => TypeToDBMap[entityType] = database;

        internal static void Clear()
        {
            TypeToDBMap.Clear();
            TypeToCollMap.Clear();
        }

        internal static IMongoDatabase GetDatabase(Type entityType)
        {
            TypeToDBMap.TryGetValue(entityType, out IMongoDatabase db);
            return db ?? DB.Database(default);
        }
    }

    internal static class Cache<T> where T : IEntity
    {
        internal static IMongoDatabase Database { get; private set; }
        internal static IMongoCollection<T> Collection { get; private set; }
        internal static string DBName { get; private set; }
        internal static string CollectionName { get; private set; }
        internal static ConcurrentDictionary<string, Watcher<T>> Watchers { get; private set; }
        internal static bool HasCreatedOn { get; private set; }
        internal static bool HasModifiedOn { get; private set; }
        internal static string ModifiedOnPropName { get; private set; }
        internal static PropertyInfo ModifiedByProp { get; private set; }

        private static PropertyInfo[] updatableProps;

        static Cache()
        {
            Initialize();
            DB.DefaultDbChanged += Initialize;
        }

        private static void Initialize()
        {
            var type = typeof(T);

            Database = TypeMap.GetDatabase(type);
            DBName = Database.DatabaseNamespace.DatabaseName;

            var collAttrb = type.GetCustomAttribute<NameAttribute>(false);
            CollectionName = collAttrb != null ? collAttrb.Name : type.Name;

            if (string.IsNullOrWhiteSpace(CollectionName) || CollectionName.Contains("~"))
                throw new ArgumentException($"{CollectionName} is an illegal name for a collection!");

            Collection = Database.GetCollection<T>(CollectionName);
            TypeMap.AddCollectionMapping(type, CollectionName);

            Watchers = new ConcurrentDictionary<string, Watcher<T>>();

            var interfaces = type.GetInterfaces();
            HasCreatedOn = interfaces.Any(it => it == typeof(ICreatedOn));
            HasModifiedOn = interfaces.Any(it => it == typeof(IModifiedOn));
            ModifiedOnPropName = nameof(IModifiedOn.ModifiedOn);

            updatableProps = type.GetProperties()
                .Where(p =>
                       p.PropertyType.Name != ManyBase.PropType &&
                      !p.IsDefined(typeof(BsonIdAttribute), false) &&
                      !p.IsDefined(typeof(BsonIgnoreAttribute), false))
                .ToArray();

            try
            {
                ModifiedByProp = updatableProps.SingleOrDefault(p =>
                                p.PropertyType == typeof(ModifiedBy) ||
                                p.PropertyType.IsSubclassOf(typeof(ModifiedBy)));
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Multiple [ModifiedBy] properties are not allowed on entities!");
            }
        }

        public static IEnumerable<PropertyInfo> UpdatableProps(T entity)
        {
            return updatableProps.Where(p =>
                !(p.IsDefined(typeof(BsonIgnoreIfDefaultAttribute), false) && p.GetValue(entity) == default) &&
                !(p.IsDefined(typeof(BsonIgnoreIfNullAttribute), false) && p.GetValue(entity) == null));
        }
    }

    internal class IgnoreManyPropertiesConvention : ConventionBase, IMemberMapConvention
    {
        public void Apply(BsonMemberMap mMap)
        {
            if (mMap.MemberType.Name == ManyBase.PropType)
            {
                _ = mMap.SetShouldSerializeMethod(_ => false);
            }
        }
    }
}