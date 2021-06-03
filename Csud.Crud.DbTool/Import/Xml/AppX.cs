using System.Xml.Linq;
using Csud.Crud.Models.App;
using Csud.Crud.Models.Internal;


namespace Csud.Crud.DbTool.Import.Xml
{
    internal sealed class AppX: App, INoneRepo
    {
        internal AppX(XElement node)
        {
            XmlGuid = node.Attribute("Guid")?.Value;
            Name = node.Attribute("Name")?.Value;
            DisplayName = node.Attribute("Description")?.Value.ExtractArgument("DisplayName", true);
        }
    }
}
