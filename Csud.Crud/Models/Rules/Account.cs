using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using Csud.Crud.Services;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Models.Rules
{
    internal class AccountValidationAttribute : BaseValidator
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Reset();
            var account = (Account)value;

            var serviceAccProvider = (IEntityService<AccountProvider>)validationContext.GetService(typeof(IEntityService<AccountProvider>));
            if (serviceAccProvider == null)
                throw new ApplicationException($"{nameof(IEntityService<AccountProvider>)} is not found");
            if (!serviceAccProvider.Select().Any(x => x.Key == account.AccountProviderKey))
                return new ValidationResult("Неверный ключ поставщика услуг.");

            var serviceSubject = (IEntityService<Subject>)validationContext.GetService(typeof(IEntityService<Subject>));
            if (serviceSubject == null)
                throw new ApplicationException($"{nameof(IEntityService<Subject>)} is not found");
            if (!serviceSubject.Select().Any(x => x.Key == account.SubjectKey))
                return new ValidationResult("Неверный ключ субъекта");

            var servicePerson = (IEntityService<Person>)validationContext.GetService(typeof(IEntityService<Person>));
            if (servicePerson == null)
                throw new ApplicationException($"{nameof(IEntityService<Subject>)} is not found");
            if (!servicePerson.Select().Any(x => x.Key == account.PersonKey))
                return new ValidationResult("Неверный ключ персоны");

            return null;
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

    public class AccountEdit : Account, IEditable
    {
        [JsonIgnore]
        public override int Key { get; set; }
    }

    public class AccountAdd : AccountEdit, IAddable
    {

    }
}
