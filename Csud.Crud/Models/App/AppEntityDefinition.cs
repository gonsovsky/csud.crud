using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csud.Crud.Models.App
{
    public class AppEntityDefinition: AppBase
    {
        public int EntityKey { get; set; }

        public int ObjectKey { get; set; }

        public string EntityName { get; set; }

        public override int UseKey
        {
            get => EntityKey;
            set => EntityKey = value;
        }
    }
}
