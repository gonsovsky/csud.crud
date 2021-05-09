using System.ComponentModel.DataAnnotations;

namespace Csud.Crud.Models.Rules
{
    public class AccountProvider : Base
    {
        [Required]
        public string Type { get; set; }
    }
}
