using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Models.App
{
    public class AppDistrib: AppBase
    {
        public int AppKey { get; set; }

        public int LoadDate { get; set; }

        public void SetDate()
        {
            LoadDate = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public string Version { get; set; }

        public virtual string DisplayName { get; set; }
    }
}
