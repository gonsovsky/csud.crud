using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Mongo;
using Csud.Crud.Postgre;
using Microsoft.EntityFrameworkCore;

namespace Csud.Crud
{
    public interface ICsud
    {
        void AddEntity<T>(T entity) where T : Base;

        void AddPerson(Person person);

        void AddAccountProvider(AccountProvider provider);

        void AddAccount(Account account);

        void AddSubject(Subject subject);

        void AddContext(Context context);

        void AddTimeContext(TimeContext timeContext);

        void AddSegmentContext(TimeContext segmentContext);

        static ICsud GetDatabase()
        {
            ICsud csud = null;
            {
#if (Postgre)
            csud = new CsudPostgre(new DbContextOptions<CsudPostgre>() { });
#else
             var mongo = new CsudMongo("127.0.0.1", "csud");
             csud = mongo;
#endif
             return csud;
            }
        }
    }
}
