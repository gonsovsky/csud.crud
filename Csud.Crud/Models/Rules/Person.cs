using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Csud.Crud.Models.Internal;

namespace Csud.Crud.Models.Rules
{
    public class Person: Base
    {
        protected override string QueueName => "Person";

        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class PersonEdit : Person, IEditable
    {
        [JsonIgnore]
        public override int Key { get; set; }
    }

    public class PersonAdd : Person, IAddable
    {

    }
}
