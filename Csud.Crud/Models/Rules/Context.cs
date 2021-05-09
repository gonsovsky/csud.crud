using System.ComponentModel.DataAnnotations.Schema;
using Csud.Crud.Models.Contexts;
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

            if (context.ContextType != Const.Context.Composite 
                && context.ContextType != Const.Context.Attrib
                && context.ContextType != Const.Context.Rule
                && context.ContextType != Const.Context.Segment
                && context.ContextType != Const.Context.Struct
                )
            {
                Error("Неверный тип контекста.");
            }
            return Validated;
        }
    }

    [ContextValidation]

    public class Context: Base
    {
        public string ContextType { get; set; }
        public bool Temporary { get; set; }
        public int HashCode { get; set; }

        [NotMapped]
        [Ignore]
        [BsonIgnore]
        public BaseContext Details { get; set; }
    }
}
