using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csud.Crud.Models.App
{
    public class AppOperationDefinition: AppBase
    {
        public int OperationKey { get; set; }

        public int ObjectKey { get; set; }

        public string OperationName { get; set; }

        public override int UseKey
        {
            get => OperationKey;
            set => OperationKey = value;
        }
    }
}
