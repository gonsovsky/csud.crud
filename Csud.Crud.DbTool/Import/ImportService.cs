using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Csud.Crud.DbTool.Import.Xml;
using Csud.Crud.DbTool.Log;
using Csud.Crud.Models.App;
 using Csud.Crud.Models.Rules;
 using Csud.Crud.Storage;

namespace Csud.Crud.DbTool.Import
{
    internal interface IImportService
    {
        public void Run(string file);
    };

    internal class ImportService : IImportService
    {
        protected static IDbService Db;

        protected ILogService Logger;

        protected XDocument Doc;

        public ImportService(IDbService db, ILogService logger)
        {
            Db = db;
            Logger = logger;
        }

        public void Run(string file)
        {
            Doc = XDocument.Load(file);
            var azAdminManager = Doc.Root;
            var azApplication = Doc.Descendants("AzApplication").First();

            //stub object
            var obj = new ObjectX() {Key = 888};

            //Distributive & Application
            var app = Db.Add(new AppX(azApplication) as App);
            var distributive = Db.Add(new AppDistribX(azAdminManager, azApplication, app) as AppDistrib);
            app.LastDistribKey = distributive.Key;
            Db.Update(app);

            //Operations
            var azOperations = azApplication.GetItems("AzOperation");
            foreach (var opNode in azOperations)
            {
                var def = Db.Add(new AppOperationDefinitionX(opNode, obj) as AppOperationDefinition);
                Db.Add(new AppOperationX(opNode, distributive, def) as AppOperation, false);
            }

            //Roles & Role Details
            IEnumerable<XElement> ExpandTasks(XElement taskNode) =>
                azApplication.Expand(taskNode, "TaskLink", "AzTask");

            IEnumerable<XElement> ExpandOperations(XElement taskNode) =>
                azApplication.Expand(taskNode, "OperationLink", "AzOperation");

            var azTasks = azApplication.GetItems("AzTask");
            foreach (var roleNode in azTasks)
            {
                var def = Db.Add(new AppRoleDefinitionX(roleNode, obj) as AppRoleDefinition);
                var role = Db.Add(new AppRoleX(roleNode, distributive, def) as AppRole, false);

                var subRoles = roleNode.Traverse(ExpandTasks)
                    .DistinctBy(a => a.Guid());

                var subOperations = subRoles
                    .SelectMany(ExpandOperations)
                    .DistinctBy(a => a.Guid());

                foreach (var op in subOperations)
                {
                    var operation = Db.AppOperation
                        .First(a=> a.XmlGuid == op.Guid());
                    Db.Add(new AppRoleDetailsX(role, operation) as AppRoleDetails);
                }
            }
        }
    }
}