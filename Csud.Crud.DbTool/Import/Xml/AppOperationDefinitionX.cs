using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Csud.Crud.Models.App;
using Csud.Crud.Models.Internal;
using Csud.Crud.Models.Rules;

namespace Csud.Crud.DbTool.Import.Xml
{
    internal class AppOperationDefinitionX: AppOperationDefinition, INoneRepo
    {
        internal AppOperationDefinitionX(XElement node, ObjectX obj)
        {
            XmlGuid = node.Attribute("Guid")?.Value;
            OperationName = node.Attribute("Name")?.Value;
            DisplayName = node.Attribute("Description")?.Value.ExtractArgument("Description", true);
            OperationId = int.Parse(node.Elements().First(a => a.Name == "OperationID").Value);

            ObjectKey = obj.Key;
        }
    }
}
