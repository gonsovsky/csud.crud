using System.Linq;
using System.Xml.Linq;
using Csud.Crud.Models.App;

namespace Csud.Crud.DbTool.Import.Xml
{
    internal sealed class AppRoleDetailsX : AppRoleDetails
    {
        internal AppRoleDetailsX(AppRole role, AppOperation op)
        {
            OperationXmlGuid = op.XmlGuid;
            OperationKey = op.Key;
            RoleXmlGuid = role.XmlGuid;
            RoleKey = role.Key;
        }
    }
}
