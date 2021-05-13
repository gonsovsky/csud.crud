using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Models.App
{
    public class AppDistrib: AppBase
    {
        public int DistribKey { get; set; }

        public int AppKey { get; set; }

        public int LoadDate { get; set; }

        public int Version { get; set; }

        public override int UseKey
        {
            get => DistribKey;
            set => DistribKey = value;
        }
    }
}
