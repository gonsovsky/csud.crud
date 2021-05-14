using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csud.Crud.Models.App
{
    public class AppAttributeDefinition: AppBase
    {
        public int AttributeKey { get; set; }

        public int ObjectKey { get; set; }

        public string EntityName { get; set; }

        public override int UseKey
        {
            get => AttributeKey;
            set => AttributeKey = value;
        }
    }
}
