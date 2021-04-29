using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;

namespace Csud.Crud
{
    public interface ICsud
    {
        void Add<T>(T entity) where T : Base;

        IQueryable<T> Q<T>() where T : Base;

        void AddPerson(Person person);

        void AddAccountProvider(AccountProvider provider);

        void AddAccount(Account account);

        void AddSubject(Subject subject);

        void AddContext(Context context);

        void AddTimeContext(TimeContext timeContext);

        void AddSegmentContext(TimeContext segmentContext);
    }
}
