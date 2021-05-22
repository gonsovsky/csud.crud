using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Csud.Crud.Services;

namespace Csud.Crud.Models.Rules
{
    public class AccountProvider : Base
    {
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
