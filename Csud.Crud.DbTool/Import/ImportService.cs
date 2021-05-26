 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Csud.Crud.DbTool.Import.Xml;
using Csud.Crud.Models.App;
 using Csud.Crud.Models.Rules;
 using Csud.Crud.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MongoDB.Bson;

namespace Csud.Crud.DbTool.Import
{
    public interface IImportService
    {
        public void Run(string file);
    };

    public class ImportService : IImportService
    {
        public static IDbService Db;

        public XDocument Doc;

        public ImportService(IDbService db)
        {
            Db = db;
        }

        public void Run(string file)
        {
            Doc = XDocument.Load(file);

            //stub object
            var obj = new ObjectX() {Key = 888};
                
            //Distrib & Application
            var azAdminManager = Doc.Root;
            var azApplication = Doc.Descendants("AzApplication").First();
            var app = new AppX(azApplication);
            var distrib = new AppDistribX(azAdminManager, azApplication, app);
            app.LastDistribKey = distrib.Key; Db.Update((App) app);

            //Operations
            var azOperations = Doc.Descendants("AzOperation");
            foreach (var node in azOperations)
            {
                var operation = new AppOperationX(node, distrib);
                new AppOperationDefinitionX(node, operation, obj);
            }

            //Roles & Role Details
            var azTasks = Doc.Descendants("AzTask");
            foreach (var node in azTasks)
            {
                var role = new AppRoleX(node, distrib);
                new AppRoleDefinitionX(node, role, obj);

                var opLinks = node.Descendants("OperationLink");
                foreach (var opLink in opLinks)
                {
                    new AppRoleDetailsX(opLink, role);
                }
            }
        }
    }
}
