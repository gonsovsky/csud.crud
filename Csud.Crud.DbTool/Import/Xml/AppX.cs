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
            try
            {
                var x = node.Attribute("Description")?.Value;
                var y = x.Split(';');
                DisplayName = y[0].Split('=')[1];
            }
            catch (Exception e)
            {
                DisplayName = "unknown name";
            }
            ImportService.Db.Add((App)this);
        }
    }
}
