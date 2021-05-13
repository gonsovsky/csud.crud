using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csud.Crud.Models.App
{
    public class App: AppBase
    {
        public int AppKey { get; set; }

        public string Name { get; set; }

        public int LastDistribKey { get; set; }
    }
}
