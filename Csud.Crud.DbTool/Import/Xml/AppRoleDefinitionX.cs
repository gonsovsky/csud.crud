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
        public AppRoleDefinitionX(XElement node, ObjectX obj)
        {
            XmlGuid = node.Attribute("Guid")?.Value;
            RoleName = node.Attribute("Name")?.Value;
            DisplayName = AppX.ExtractDisplayName(node.Attribute("Description")?.Value);
            ObjectKey = obj.Key;
            ImportService.Db.Add((AppRoleDefinition)this, true);
        }
    }
}
