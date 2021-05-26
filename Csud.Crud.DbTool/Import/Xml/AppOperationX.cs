using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Csud.Crud.Models.App;

namespace Csud.Crud.DbTool.Import.Xml
{
    public class AppOperationX: AppOperation
    {
        public AppOperationX(XElement node, AppDistrib distrib)
        {
            XmlGuid = node.Attribute("Guid")?.Value;
            OperationName = node.Attribute("Name")?.Value;
            DisplayName = node.Attribute("Description")?.Value;
            var operationId = node.Elements().First(a => a.Name == "OperationID");
            Key = int.Parse(operationId.Value);
            DistribKey = distrib.Key;
            ImportService.Db.Add((AppOperation)this, false);
        }
    }
}
