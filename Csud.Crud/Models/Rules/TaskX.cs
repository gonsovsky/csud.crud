using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Csud.Crud.Models.Rules
{
    internal class TaskValidator : BaseValidator
    {
        public override bool IsValid(object value)
        {
            Reset();
            if (value is IRelational group)
            {
                if (value is IRelationalAdd)
                {
                    foreach (var rkey in group.RelatedKeys)
                    {
                        if (Csud.Object.Any(a => a.Key == rkey) == false)
                            Error($"Связанный объект с кодом {rkey} не найден");
                    }
                }
                else
                {
                    if (!Csud.Object.Any(x => x.Key == group.RelatedKey))
                    {
                        Error("Неверный код связанного объекта.");
                    }
                }
            }
            return Validated;
        }
    }

    [TaskValidator]
    public class TaskX : Base, IRelational
    {
        public int? RelatedKey { get; set; }
        [NotMapped] [BsonIgnore] [Ignore] [JsonIgnore] public List<int?> RelatedKeys { get; set; } = new List<int?>();
        [NotMapped] [BsonIgnore] [Ignore] [JsonIgnore] public IEnumerable RelatedEntities { get; set; }
    }

    public class TaskAdd : TaskX, IRelationalAdd
    {
        [JsonIgnore] protected new int? Key { get; set; }
        [JsonIgnore] protected new int? RelatedKey { get; set; }
        public new List<int?> RelatedKeys { get; set; }
    }
}
