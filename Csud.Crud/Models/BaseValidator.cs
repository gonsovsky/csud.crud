using System.ComponentModel.DataAnnotations;

namespace Csud.Crud.Models
{
    internal class BaseValidator : ValidationAttribute
    {
        protected void Error(string err)
        {
            this.ErrorMessage ??= "";
            this.ErrorMessage += @$"{err} ";
        }
        protected void Reset()
        {
            this.ErrorMessage = null;
        }
        protected bool Validated => string.IsNullOrEmpty(this.ErrorMessage);
    }
}
