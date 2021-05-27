using System.Xml.Linq;
using Csud.Crud.Models.App;
using Csud.Crud.Models.Internal;
using Csud.Crud.Models.Rules;

namespace Csud.Crud.DbTool.Import.Xml
{
    internal sealed class AppRoleDefinitionX: AppRoleDefinition, INoneRepo
    {
        internal AppRoleDefinitionX(XElement node, ObjectX obj)
        {
            XmlGuid = node.Attribute("Guid")?.Value;
            RoleName = node.Attribute("Name")?.Value;
            DisplayName = node.Attribute("Description")?.Value.ExtractArgument("DisplayName", true);
            Description = node.Attribute("Description")?.Value.ExtractArgument(-1);
            RoleContext = node.Attribute("Description")?.Value.ExtractArgumentInt("Context", false);
            RoleRule = node.Attribute("Description")?.Value.ExtractArgumentInt("Rule", false);

            ObjectKey = obj.Key;
        }
    }
}
