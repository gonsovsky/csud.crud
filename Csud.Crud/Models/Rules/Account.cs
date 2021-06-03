using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Csud.Crud.Models.Internal;
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

            if (value is IAddable)
            {
                var serviceAccProvider =
                    (IEntityService<AccountProvider>) validationContext.GetService(
                        typeof(IEntityService<AccountProvider>));
                if (serviceAccProvider == null)
                    throw new ApplicationException($"{nameof(IEntityService<AccountProvider>)} is not found");
                if (!serviceAccProvider.Select().Any(x => x.Key == account.ProviderKey))
                    return new ValidationResult("Неверный ключ поставщика услуг.");
            }

            if (value is IAddable || FieldDefined(account.SubjectKey))
            {
                var serviceSubject =
                    (IEntityService<Subject>) validationContext.GetService(typeof(IEntityService<Subject>));
                if (serviceSubject == null)
                    throw new ApplicationException($"{nameof(IEntityService<Subject>)} is not found");
                if (!serviceSubject.Select().Any(x => x.Key == account.SubjectKey))
                    return new ValidationResult("Неверный ключ субъекта");
            }

            if (value is IAddable || FieldDefined(account.PersonKey))
            {
                var servicePerson =
                    (IEntityService<Person>) validationContext.GetService(typeof(IEntityService<Person>));
                if (servicePerson == null)
                    throw new ApplicationException($"{nameof(IEntityService<Subject>)} is not found");
                if (!servicePerson.Select().Any(x => x.Key == account.PersonKey))
                    return new ValidationResult("Неверный ключ персоны");
            }

            return null;
        }
    }

    [AccountValidation]
    public class Account: Base, IWellNamed
    {
        protected override string QueueName => "Account";
        public int PersonKey { get; set; }
        public int SubjectKey { get; set; }

        [NotMapped] [BsonIgnore] [JsonIgnore] public AccountProvider AccountProvider
        {
            set => ProviderKey = value.Key;
        }
        [NotMapped] [BsonIgnore] [JsonIgnore] public Subject Subject
        {
            set => SubjectKey = value.Key;
        }
        [NotMapped] [BsonIgnore] [JsonIgnore] public Person Person
        {
            set => PersonKey = value.Key;
        }

        [NotMapped] [BsonIgnore] [JsonIgnore] protected new string Key
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public virtual string AccountKey { get; set; }
        public virtual int ProviderKey { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
    }

    public class AccountEdit : Account, IEditable
    {
        [JsonIgnore] public override string AccountKey { get; set; }

        [JsonIgnore] public override int ProviderKey { get; set; }
    }

    public class AccountAdd : Account, IAddable
    {
        [JsonIgnore] public override string AccountKey { get; set; }
    }

    public class AccountKey : IEntityKey
    {
        [DisplayName("account")]
        [DataMember(Name = "account")]
        [JsonPropertyName("account")]
        public virtual string Account { get; set; }

        public virtual int Provider { get; set; }

        public void CopyTo(Base entity)
        {
            if (entity is not Account acc) 
                return;
            acc.ProviderKey = Provider;
            acc.AccountKey = Account;
        }
        public virtual IEntityKey CopyFrom(Base entity)
        {
            if (entity is not Account acc)
                return null;
            return new AccountKey() { Account = acc.AccountKey, Provider = acc.ProviderKey};
        }
    }

}
