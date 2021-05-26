using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Csud.Crud.Models.App;

namespace Csud.Crud.DbTool.Import.Xml
{
    public class AppRoleDetailsX: AppRoleDetails
    {
        public AppRoleDetailsX(XElement node, AppRole role)
        {
            XmlGuid = node.Value;
            OperationXmlGuid = XmlGuid;
            RoleXmlGuid = role.XmlGuid;
            RoleKey = role.Key;
            OperationKey = ImportService.Db.AppOperation.First(x => x.XmlGuid == OperationXmlGuid).Key;
            ImportService.Db.Add((AppRoleDetails) this, true);
        }
    }
}
