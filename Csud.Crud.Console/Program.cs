using System;
using System.Configuration;
using System.Threading.Tasks;
using Csud.Crud;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Mongo;
#if (Postgre)
using Csud.Crud.Postgre;
#endif
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Entities;

namespace Csud.Crud.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ICsud csud = null;
#if (Postgre)
            csud = new MyDbContext(new DbContextOptions<MyDbContext>() { });
#else
            var mongo = new CsudMongo();
            mongo.StartUp("127.0.0.1", "csud");
            csud = mongo;
            
#endif
            var gen = new DatasetGenerator(csud);
            gen.Generate();
        }

        static void MainPostgre()
        {
            using (var context = new MyDbContext(new DbContextOptions<MyDbContext>(){}))
            {
                var person1 = new Person() { FirstName = "PersonA", LastName = "" };
                context.Person.Add(person1);
                context.SaveChanges();
            }
        }

        static async Task MainMongo()
        {
            var mongo = new CsudMongo();
            mongo.StartUp("127.0.0.1", "csud");

            //Person
            var person1 = new Person() {FirstName = "PersonA", LastName = ""};
            await person1.SaveAsync();
            var person2 = new Person() {FirstName = "PersonB", LastName = ""};
            await person2.SaveAsync();

            //AccountProvider
            var ap1 = new AccountProvider() {Type = "DC"};
            await ap1.SaveAsync();
            var ap2 = new AccountProvider() {Type = "Odnoklassniki"};
            await ap2.SaveAsync();

            //Account
            var account1 = new Account() {Name = "Acc1", Person = person1, AccountProvider = ap1};
            await account1.SaveAsync();
            var account2 = new Account() {Name = "Acc2", Person = person2, AccountProvider = ap2};
            await account2.SaveAsync();

            //Context
            var context1 = new Context() {Type = Context.ContextTypeEnum.Time, DisplayName = "context1"};
            await context1.SaveAsync();
            var context2 = new Context() {Type = Context.ContextTypeEnum.Time, DisplayName = "context2"};
            await context2.SaveAsync();

            //timeContext
            var timeContext1 = new TimeContext()
                {Context = context1, TimeStart = new TimeSpan(8, 0, 0), TimeEnd = new TimeSpan(15, 0, 0)};
            await timeContext1.SaveAsync();
            var timeContext2 = new TimeContext()
                {Context = context2, TimeStart = new TimeSpan(15, 0, 1), TimeEnd = new TimeSpan(20, 0, 0)};
            await timeContext2.SaveAsync();

            //subject
            var subject1 = new Subject() {Context = context1, Name = "subject1"};
            await subject1.SaveAsync();
            var subject2 = new Subject() {Context = context2, Name = "subject2"};
            await subject2.SaveAsync();

            account1.Subject = subject1;
            account2.Subject = subject2;
            await account1.SaveAsync();
            await account2.SaveAsync();

            //object
            var obj1 = new Obj() {Context = context1, Name = "object1"};
            await obj1.SaveAsync();
            var obj2 = new Obj() {Context = context2, Name = "object2"};
            await obj2.SaveAsync();

            //task
            var task1 = new TaskX() {Object = obj1, Name = "task1"};
            await task1.SaveAsync();
            var task2 = new TaskX() {Object = obj2, Name = "task2"};
            await task2.SaveAsync();

            var account = DB.Collection<Account>().AsQueryable();
            var accountProvider = DB.Collection<AccountProvider>().AsQueryable();
            var context = DB.Collection<Context>().AsQueryable();
            var obj = DB.Collection<Obj>().AsQueryable();
            var person = DB.Collection<Person>().AsQueryable();
            var subject = DB.Collection<Subject>().AsQueryable();
            var task = DB.Collection<TaskX>().AsQueryable();
            var timeContext = DB.Collection<TimeContext>().AsQueryable();

            //var join = from s in subject
            //    join a in account on s.ID equals a.Subject.ID
            //    join ap in accountProvider on s.ID equals ap.ID
            //    join p in person on a.Person.ID equals p.ID
            //    join co in context on s.Context.ID equals co.ID
            //    join tc in timeContext on co.ID equals tc.Context.ID
            //    join o in obj on co.ID equals o.Context.ID
            //    join t in task on o.ID equals t.Object.ID
            //    select new
            //    {
            //        SubjectKey = s.ID.ToString(),
            //        Subject = s.Name.ToString(),
            //        Account = a.Name,
            //        Provider = ap.Type,
            //        Person = p.FirstName,
            //        ContextType = co.Type,
            //        TimeStart = tc.TimeStart,
            //        TimeEnd = tc.TimeEnd,
            //        ObjectName = o.Name,
            //        ObjectType = o.Type,
            //        Task = t.Name
            //    };



            System.Console.WriteLine("enter to quit");
            System.Console.ReadKey();
        }
    }
}
