using System.Linq;
using System.Xml.Linq;
using Csud.Crud.Models.App;
using Csud.Crud.Models.Internal;

namespace Csud.Crud.DbTool.Import.Xml
{
    internal sealed class AppOperationX: AppOperation, INoneRepo
    {
        internal AppOperationX(XElement node, AppDistrib distrib, AppOperationDefinition def)
        {
            XmlGuid = def.XmlGuid;
            OperationName = def.OperationName;
            DisplayName = def.DisplayName;
            OperationId = def.OperationId;

            Key = def.Key;
            DistribKey = distrib.Key;
        }
    }
}
