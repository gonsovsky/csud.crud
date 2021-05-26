using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Models.App
{
    public class AppRoleDetails: AppBase
    {
        public int RoleKey { get; set; }

        public int OperationKey { get; set; }

        public string RoleXmlGuid { get; set; }

        public string OperationXmlGuid { get; set; }

        [NotMapped] [JsonIgnore] [BsonIgnore] protected new int Key { get; set; }

        [NotMapped] [JsonIgnore] [BsonIgnore] protected new string XmlGuid { get; set; }

        protected override string QueueName => nameof(AppRoleDetails);

    }
}
