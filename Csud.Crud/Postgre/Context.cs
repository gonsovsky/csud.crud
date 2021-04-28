using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Csud.Crud.Postgre
{
    public sealed class MyDbContext : DbContext, ICsud
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"Server=localhost;database=postgres;user id=postgres;password=abc123");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .Property(f => f.ID)
                .ValueGeneratedOnAdd();

            /*
            modelBuilder.Entity<AccountProvider>()
                .Property(f => f.ID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Account>()
                .Property(f => f.ID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Context>()
                .Property(f => f.ID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Subject>()
                .Property(f => f.ID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Obj>()
                .Property(f => f.ID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<TaskX>()
                .Property(f => f.ID)
                .ValueGeneratedOnAdd();*/
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

        public void AddAccount(Account account)
        {
            if (account.HasProvider == false)
            {
                var provider = new AccountProvider() {Name = "DC", DisplayName = "Active Directory"};
                AccountProvider.Add(provider);
                SaveChanges();
                account.AccountProvider = provider;
            }
        }
    }
}
