using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Csud.Crud.Models.App;

namespace Csud.Crud.DbTool.Import.Xml
{
    public class AppRoleX: AppRole
    {
        public AppRoleX(XElement node, AppDistrib distrib)
        {
            XmlGuid = node.Attribute("Guid")?.Value;
            RoleName = node.Attribute("Name")?.Value;
            DisplayName = node.Attribute("Description")?.Value;
            DistribKey = distrib.Key;
            ImportService.Db.Add((AppRole)this, true);
        }
    }
}
