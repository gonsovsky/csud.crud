using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Models.App;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Models.Maintenance;
using Csud.Crud.Models.Rules;
using Microsoft.EntityFrameworkCore;

namespace Csud.Crud.Storage
{
    public class PostgreService : DbContext, IDbService
    {
        private Config config;

        public PostgreService(Config cfg)
        {
            this.config = cfg;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(config.Postgre.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompositeContext>()
                .HasKey(x => new { x.Key, x.RelatedKey });

            modelBuilder.Entity<Group>()
                .HasKey(x => new { x.Key, x.RelatedKey });

            modelBuilder.Entity<TaskX>()
                .HasKey(x => new { x.Key, x.RelatedKey });

            modelBuilder.Entity<Account>()
                .HasKey(x => new { x.Key, x.AccountProviderKey });

            modelBuilder.Entity<Account>()
                .Property(f => f.Key)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Person>()
                .HasIndex(x => new { x.FirstName, x.LastName });

            modelBuilder.Entity<Relation>()
                .HasKey(x => new {x.Key});

            modelBuilder.Entity<RelationDetails>()
                .HasKey(x => new { x.RelatedKey, x.ObjectKey, x.SubjectKey });

            modelBuilder.Entity<App>().HasKey(x => x.AppKey);

            modelBuilder.Entity<AppDistrib>().HasKey(x => new { x.AppKey, x.DistribKey });

            modelBuilder.Entity<AppDistrib>()
                .Property(f => f.DistribKey)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<AppRole>().HasKey(x => new { x.DistribKey, x.RoleName });

            modelBuilder.Entity<AppRoleDefinition>().HasKey(x => x.RoleKey);

            modelBuilder.Entity<AppRoleDetails>().HasKey(x => new { x.RoleKey, x.OperationKey });

            modelBuilder.Entity<AppEntityDefinition>().HasKey(x => x.EntityKey);

            modelBuilder.Entity<AppEntity>().HasKey(x => new { x.DistribKey, x.EntityName });

            modelBuilder.Entity<AppAttributeDefinition>().HasKey(x => x.AttributeKey);

            modelBuilder.Entity<AppAttribute>().HasKey(x => new { x.DistribKey, x.AttributeType });

            modelBuilder.Entity<AppOperationDefinition>().HasKey(x => x.OperationName);

            modelBuilder.Entity<AppOperation>().HasKey(x => new { x.DistribKey, x.OperationName });

            modelBuilder.Entity<AppImport>().HasKey(x => x.Key);
        }

        public DbSet<Person> Person { get; set; }
        public DbSet<AccountProvider> AccountProvider { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Context> Context { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<ObjectX> ObjectX { get; set; }
        public DbSet<TaskX> Task { get; set; }
        public DbSet<CompositeContext> CompositeContext { get; set; }
        public DbSet<RuleContext> RuleContext { get; set; }
        public DbSet<SegmentContext> SegmentContext { get; set; }
        public DbSet<StructContext> StructContext { get; set; }
        public DbSet<TimeContext> TimeContext { get; set; }
        public DbSet<RelationDetails> RelationDetails { get; set; }
        public DbSet<Relation> Relation { get; set; }
        public DbSet<App> App { get; set; }
        public DbSet<AppDistrib> AppDistrib { get; set; }
        public DbSet<AppRoleDefinition> AppRoleDefinition { get; set; }
        public DbSet<AppRole> AppRole { get; set; }
        public DbSet<AppRoleDetails> AppRoleDetails { get; set; }
        public DbSet<AppEntityDefinition> AppEntityDefinition { get; set; }
        public DbSet<AppEntity> AppEntity { get; set; }
        public DbSet<AppAttributeDefinition> AppAttributeDefinition { get; set; }
        public DbSet<AppAttribute> AppAttribute { get; set; }
        public DbSet<AppOperationDefinition> AppOperationDefinition { get; set; }
        public DbSet<AppOperation> AppOperation { get; set; }
        public DbSet<AppImport> AppImport { get; set; }

        public void Add<T>(T entity, bool generateKey = true) where T : Base
        {
            if (generateKey)
                entity.Key = 0;
            Set<T>().Add(entity);
            SaveChanges();
        }

        public new void Update<T>(T entity) where T : Base
        {
            Set<T>().Update(entity);
            SaveChanges();
        }
        
        public IQueryable<T> Select<T>(string status = Const.Status.Actual) where T : Base
        {
            return Set<T>().AsQueryable().Where(x => x.Status == status);
        }
      
    }
}
