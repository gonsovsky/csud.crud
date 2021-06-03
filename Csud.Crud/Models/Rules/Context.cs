using System.ComponentModel.DataAnnotations.Schema;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Models.Internal;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Csud.Crud.Models.Rules
{
    internal class ContextValidationAttribute : BaseValidator
    {
        public override bool IsValid(object value)
        {
            Reset();
            var context = (Context)value;
            if (!Const.Context.Has(context.ContextType))
            {
                Error("Неверный тип контекста.");
            }
            return Validated;
        }
    }

    [ContextValidation]
    public class Context: Base, IWellNamed
    {

        public string ContextType { get; set; }
        public bool Temporary { get; set; }
        public int HashCode { get; set; }

        [NotMapped] [Ignore] [BsonIgnore]
        public BaseContext Details { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        protected override string QueueName => "Context";
    }
}
