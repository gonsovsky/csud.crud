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
    public class AppDistribX : AppDistrib, INoneRepo
    {
        public AppDistribX(XElement rootNode, XElement appNode, App app)
        {
            XmlGuid = rootNode.Attribute("Guid")?.Value;
            DisplayName = rootNode.Attribute("Description")?.Value;
            var version = "";
            try
            {
                var version1 = appNode.Attribute("ApplicationVersion")?.Value;
                var version2 = appNode.Attribute("Description")?.Value;

                if (!string.IsNullOrEmpty(version1))
                    version = version1;
                else if (string.IsNullOrEmpty(version2))
                    version = "unknown version";
                else
                {
                    var x = version2.Split(';');
                    version = x[1].Split('=')[1].Trim();
                }
            }
            catch (Exception e)
            {
                version = "unknown version";
            }
            Version = version;
            AppKey = app.Key;
            SetDate();
            ImportService.Db.Add((AppDistrib)this);
        }
    }
}
