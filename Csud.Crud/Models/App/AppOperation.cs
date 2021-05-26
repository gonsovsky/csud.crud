using System;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Models.App
{
    public class AppOperation: AppBase
    {
        public override int Key { get; set; }

        public int DistribKey { get; set; }

        public string OperationName { get; set; }

        public string DisplayName { get; set; }

        protected override string QueueName => nameof(AppOperation);
    }
}
