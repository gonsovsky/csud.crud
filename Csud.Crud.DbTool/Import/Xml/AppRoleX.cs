using System.Linq;
using System.Xml.Linq;
using Csud.Crud.Models.App;

namespace Csud.Crud.DbTool.Import.Xml
{
    internal sealed class AppRoleX: AppRole
    {
        internal AppRoleX(XElement node, AppDistrib distrib, AppRoleDefinition def)
        {
            XmlGuid = def.XmlGuid;
            RoleName = def.RoleName;
            DisplayName = def.DisplayName;
            Description = def.Description;
            RoleContext = def.RoleContext;
            RoleRule = def.RoleRule;

            DistribKey = distrib.Key;
            Key = def.Key;
        }
    }
}
