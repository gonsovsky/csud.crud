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
            //modelBuilder.Entity<Person>()
            //    .Property(f => f.ID)
            //    .ValueGeneratedOnAdd();
        }

        public DbSet<Person> Person { get; set; }
        public DbSet<AccountProvider> AccountProvider { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Context> Context { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<Obj> Obj { get; set; }
        public DbSet<TaskX> Task { get; set; }

        public DbSet<AttribContext> AttribContext { get; set; }
        public DbSet<CompositeContext> CompositeContext { get; set; }
        public DbSet<RuleContext> RuleContext { get; set; }
        public DbSet<SegmentContext> SegmentContext { get; set; }
        public DbSet<StructContext> StructContext { get; set; }
        public DbSet<TimeContext> TimeContext { get; set; }

        public void Add<T>(T entity) where T : Base
        {
            Set<T>().Add(entity);
            SaveChanges();
        }

        public IQueryable<T> Q<T>() where T : Base
        {
            return Set<T>().AsQueryable();
        }

        public void AddPerson(Person person) => Add(person);
        public void AddAccountProvider(AccountProvider provider) => Add(provider);
        public void AddAccount(Account account) => Add(account);
        public void AddSubject(Subject subject) => Add(subject);
        public void AddContext(Context context) => Add(context);
        public void AddTimeContext(TimeContext timeContext) => Add(timeContext);
        public void AddSegmentContext(TimeContext segmentContext) => Add(segmentContext);
    }
}
