using System.ComponentModel.DataAnnotations;

namespace Csud.Crud.Models
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
    }
}
