using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;

namespace Csud.Crud.Models.Rules
{
    internal class AccountValidationAttribute : BaseValidator
    {
        public override bool IsValid(object value)
        {
            Reset();
            var account = (Account)value;
            if (!Csud.AccountProvider.Any(x => x.Key == account.AccountProviderKey))
            {
                Error("Неверный код поставщика учетных записей.");
            }
            if (!Csud.Person.Any(x => x.Key == account.PersonKey))
            {
                Error("Неверный код персоны.");
            }
            return Validated;
        }
    }

    [AccountValidation]
    public class Account: Base, INameable
    {
        public int AccountProviderKey { get; set; }
        public int PersonKey { get; set; }
        public int SubjectKey { get; set; }

        [NotMapped] [BsonIgnore] [JsonIgnore] public AccountProvider AccountProvider
        {
            set => AccountProviderKey = value.Key;
        }
        [NotMapped] [BsonIgnore] [JsonIgnore] public Subject Subject
        {
            set => SubjectKey = value.Key;
        }
        [NotMapped] [BsonIgnore] [JsonIgnore] public Person Person
        {
            set => PersonKey = value.Key;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
    }
}
