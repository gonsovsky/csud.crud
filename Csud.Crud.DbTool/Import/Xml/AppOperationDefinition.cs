using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Csud.Crud.Models.App;
using Csud.Crud.Models.Rules;

namespace Csud.Crud.DbTool.Import.Xml
{
    public class AppOperationDefinitionX: AppOperationDefinition
    {
        public AppOperationDefinitionX(XElement node, AppOperation operation, ObjectX obj)
        {
            Key = operation.Key;
            OperationName = operation.OperationName;
            ObjectKey = obj.Key;
            ImportService.Db.Add((AppOperationDefinition)this, false);
        }
    }
}
