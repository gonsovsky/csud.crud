using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Models.Rules;
using Microsoft.EntityFrameworkCore;

namespace Csud.Crud.Postgre
{
    public sealed class CsudPostgre : DbContext, ICsud
    {
        private Config config;

        public CsudPostgre(Config cfg)
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

        public void AddEntity<T>(T entity, bool generateKey = true) where T : Base
        {
            Set<T>().Add(entity);
            SaveChanges();
        }
        public void UpdateEntity<T>(T entity) where T : Base
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
