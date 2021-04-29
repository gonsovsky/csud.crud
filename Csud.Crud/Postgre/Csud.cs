using System;
using System.Collections.Generic;
using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Csud.Crud.Postgre
{
    public sealed class CsudPostgre : DbContext, ICsud
    {
        public CsudPostgre(DbContextOptions<CsudPostgre> options) : base(options)
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

        public static DbSet<T> DbSet<T>(Base entity) where T : Base
        {
            return null;
        }

        public void AddEntity<T>(T entity) where T : Base
        {
            //if (entity.HasId == false)
            //{
            //    Subject.Add(entity);
            //    SaveChanges();
            //}
        }

        public void AddPerson(Person person)
        {
            if (!person.HasId)
            {
                Person.Add(person);
                SaveChanges();
            }
        }

        public void AddAccountProvider(AccountProvider provider)
        {
            if (provider.HasId == false)
            {
                AccountProvider.Add(provider);
            }
        }

        public void AddAccount(Account account)
        {
            if (account.HasId == false)
            {
                Account.Add(account);
                SaveChanges();
            }
        }

        public void AddSubject(Subject subject)
        {
            if (subject.HasId == false)
            {
                Subject.Add(subject);
                SaveChanges();
            }
        }

        public void AddContext(Context context)
        {

        }

        public void AddTimeContext(TimeContext timeContext)
        {

        }

        public void AddSegmentContext(TimeContext segmentContext)
        {

        }

    }
}
