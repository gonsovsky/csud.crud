using System.ComponentModel.DataAnnotations;

namespace Csud.Crud.Models.Internal
{
    internal class BaseValidator : ValidationAttribute
    {
        protected void Error(string err)
        {
            ErrorMessage ??= "";
            ErrorMessage += @$"{err} ";
        }
        protected void Reset()
        {
            ErrorMessage = null;
        }
        protected bool Validated => string.IsNullOrEmpty(ErrorMessage);

        public static bool FieldDefined(object field)
        {
            var x = field switch
            {
                null => false,
                string s => !string.IsNullOrEmpty(s),
                int i => i != 0,
                _ => true
            };
            return x;
        }
    }
}
