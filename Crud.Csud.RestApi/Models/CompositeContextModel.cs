using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;

namespace Crud.Csud.RestApi.Models
{
    public class CompositeContextModel: BaseContext
    {
        public int?[] RelatedKeys { get; set; }
    }
}
