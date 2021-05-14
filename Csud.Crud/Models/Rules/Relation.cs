using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Csud.Crud.Services;
using MongoDB.Bson.Serialization.Attributes;

namespace Csud.Crud.Models.Rules
{
    public class Relation : Base, INameable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
    }
}
