using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Entities;

namespace Csud.Crud.Models
{
    public class Person: Base
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

#if (Postgre)

#else
        public override string GenerateNewID() => Next<Person>();

        public override void StartUp()
        {
            DB.Index<Person>()
                .Option(o => o.Background = false)
                .Key(a => a.FirstName, KeyType.Text)
                .Key(a => a.LastName, KeyType.Text)
                .CreateAsync().Wait();
        }
#endif
    }
}
