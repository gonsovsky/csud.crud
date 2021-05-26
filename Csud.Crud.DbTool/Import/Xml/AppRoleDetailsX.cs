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
        public AppRoleDetailsX(XElement node, AppRole role, AppOperation op)
        {
            XmlGuid = node.Value;
            OperationXmlGuid = op.XmlGuid;
            RoleXmlGuid = role.XmlGuid;
            RoleKey = role.Key;
            OperationKey = op.Key;
            if (ImportService.Db.AppRoleDetails.Any(x => x.RoleKey == RoleKey 
                                                         && x.OperationKey == OperationKey))
            {
                Console.WriteLine($"Dublicate Role details: [rolekey = {RoleXmlGuid}], [operation key = {OperationXmlGuid}]");
            }
            else
            {
                ImportService.Db.Add((AppRoleDetails)this, true);
            }
        }
    }
}
