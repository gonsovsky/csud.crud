using System.ComponentModel.DataAnnotations;

namespace Csud.Crud.Models.Rules
{
    public class Person: Base
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
