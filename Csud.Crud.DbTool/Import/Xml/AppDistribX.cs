using System;
using System.Xml.Linq;
using Csud.Crud.Models.App;
using Csud.Crud.Models.Internal;
using Csud.Crud.Services;

namespace Csud.Crud.DbTool.Import.Xml
{
    internal sealed class AppDistribX : AppDistrib, INoneRepo
    {
        internal AppDistribX(XElement rootNode, XElement appNode, App app)
        {
            XmlGuid = rootNode.Attribute("Guid")?.Value;
            DisplayName = rootNode.Attribute("Description")?.Value.ExtractArgument("Description", true);
            Version = appNode.Attribute("Description")?.Value.ExtractArgument("AppVersion", true); ;
            AppKey = app.Key;
            SetDate();
        }
    }
}
