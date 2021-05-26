using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Csud.Crud.Models.App;
using Csud.Crud.Services;


namespace Csud.Crud.DbTool.Import.Xml
{
    public class AppX: App, INoneRepo
    {
        public AppX(XElement node)
        {
            XmlGuid = node.Attribute("Guid")?.Value;
            Name = node.Attribute("Name")?.Value;
            DisplayName = ExtractDisplayName(node.Attribute("Description")?.Value);

            ImportService.Db.Add((App)this);
        }

        public static string ExtractDisplayName(string description)
        {
            var p = description.Split(';');
            if (p.Length == 0)
                return description;
            var q = p[0];
            var z = q.Split('=');
            return z.Length == 0 ? q : z[1];
        }

    }
}
