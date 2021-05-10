using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Models
{
    internal interface INameable
    {
        string Name { get; set; }
        string Description { get; set; }
        string DisplayName { get; set; }
    }

    internal interface IContextable
    {
        int? ContextKey { get; set; }
    }
}
