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
             app.LastDistribKey = distrib.Key;
             Db.Update((App) app);

             //Operations
             var azOperations = azApplication.Elements()
                 .Where(x => x.Name == "AzOperation");
             foreach (var node in azOperations)
             {
                 new AppOperationX(node, distrib);
             }

             //Roles
             var azTasks = azApplication.Elements()
                 .Where(x => x.Name == "AzTask" && x.Attribute("RoleDefinition")?.Value == "True");
             foreach (var node in azTasks)
             {
                 var def = new AppRoleDefinitionX(node, obj);
                 var azLinks = node.Elements("TaskLink");
                 foreach (var link in azLinks)
                 {
                     var taskId = link.Value;
                     var task = azApplication.Elements()
                         .First(x => x.Name == "AzTask" && x.Attribute("RoleDefinition")?.Value != "True"
                                                        && x.Attribute("Guid")?.Value == taskId);
                     AppRole role = new AppRoleX(task, distrib, def.Key);
                 }
             }

             azTasks = azApplication.Elements()
                 .Where(x => x.Name == "AzTask" && x.Attribute("RoleDefinition")?.Value != "True");
             foreach (var node in azTasks)
             {
                 var opLinks = node.Elements("OperationLink");
                 foreach (var link in opLinks)
                 {

                     if (Db.AppRole.Any(x => x.XmlGuid == node.Attribute("Guid").Value))
                     {
                         var operation = Db.AppOperation.First(x => x.XmlGuid == link.Value);
                         var role = Db.AppRole.First(x => x.XmlGuid == node.Attribute("Guid").Value);
                         new AppRoleDetailsX(link, role, operation);
                     }
                     else
                     {
                         Console.WriteLine($"Role not found: {node.Attribute("Guid")?.Value}");
                     }
                 }
             }
         }
     }
 }