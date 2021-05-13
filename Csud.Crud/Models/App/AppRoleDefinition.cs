using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csud.Crud.Models.App
{
    public class AppRoleDefinition : AppBase
    {
        public int RoleKey { get; set; }
        public int ObjectKey { get; set; }
        public string RoleName { get; set; }
        public string DisplayName { get; set; }

        public override int UseKey
        {
            get => RoleKey;
            set => RoleKey = value;
        }
    }
}
