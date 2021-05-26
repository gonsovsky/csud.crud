using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Csud.Crud.Models.App;
using Csud.Crud.Models.Rules;

namespace Csud.Crud.DbTool.Import.Xml
{
    public class AppRoleDefinitionX: AppRoleDefinition
    {
        public AppRoleDefinitionX(XElement node, AppRole role, ObjectX obj)
        {
            var def = node.Attribute("RoleDefinition")?.Value;
            if (def != "True")
                return;
            Key = role.Key;
            RoleName = role.RoleName;
            ObjectKey = obj.Key;
            DisplayName = role.DisplayName;
            ImportService.Db.Add((AppRoleDefinition)this, false);
        }
    }
}
