using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csud.Crud.Models.Rules;

namespace Csud.Crud.Models
{
    internal class BaseValidator : ValidationAttribute
    {
        internal ICsud Csud => CsudService.Csud;
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
