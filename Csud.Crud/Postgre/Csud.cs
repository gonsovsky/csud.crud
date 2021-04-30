using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Csud.Crud.Postgre
{
    public sealed class CsudPostgre : DbContext, ICsud
    {
        private readonly string conStr;

        public CsudPostgre(string conStr)
        {
            this.conStr = conStr;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(conStr);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompositeContext>()
                .HasKey(x => new { x.Key, x.RelatedKey });

            modelBuilder.Entity<Person>()
                .HasIndex(x => new { x.FirstName, x.LastName });
        }

        public DbSet<Person> Person { get; set; }
        public DbSet<AccountProvider> AccountProvider { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Context> Context { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<ObjectX> Obj { get; set; }
        public DbSet<TaskX> Task { get; set; }
        public DbSet<CompositeContext> CompositeContext { get; set; }
        public DbSet<RuleContext> RuleContext { get; set; }
        public DbSet<SegmentContext> SegmentContext { get; set; }
        public DbSet<StructContext> StructContext { get; set; }
        public DbSet<TimeContext> TimeContext { get; set; }

        public void AddEntity<T>(T entity, bool idPredefined = false) where T : Base
        {
            Set<T>().Add(entity);
            SaveChanges();
        }

        public void UpdateEntity<T>(T entity) where T : Base
        {
            Set<T>().Update(entity);
            SaveChanges();
        }

        public IQueryable<T> Q<T>() where T : Base
        {
            return Set<T>().AsQueryable();
        }
    }
}
