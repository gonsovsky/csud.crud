using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Csud.Crud.Models.Internal;
using Csud.Crud.Services;

namespace Csud.Crud.Models.Rules
{
    public class AccountProvider : Base
    {
        protected override string QueueName => "AccountProvider";

        [Required]
        public string Type { get; set; }
    }

    public class AccountProviderEdit : AccountProvider, IEditable
    {
        [JsonIgnore]
        public override int Key { get; set; }
    }

    public class AccountProviderAdd : AccountProviderEdit, IAddable
    {

    }
}
