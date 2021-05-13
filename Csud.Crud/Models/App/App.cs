﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Models.App
{
    public class App: AppBase
    {
        public int AppKey { get; set; }

        public string Name { get; set; }

        public int LastDistribKey { get; set; }

        public override int UseKey
        {
            get => AppKey;
            set => AppKey = value;
        }

    }
}
