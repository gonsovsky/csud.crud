using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using Csud.Crud.Services;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Csud.Crud.Models.Rules
{
    internal class TaskValidator : BaseValidator
    {
        public override bool IsValid(object value)
        {
            Reset();
            if (value is IOneToMany group)
            {
                if (value is IOneToManyAdd)
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
    public class TaskX : Base, IOneToMany
    {
        public int RelatedKey { get; set; }
        [NotMapped] [BsonIgnore] [Ignore] [JsonIgnore] public List<int> RelatedKeys { get; set; } = new List<int>();
        [NotMapped] [BsonIgnore] [Ignore] [JsonIgnore] public IEnumerable RelatedEntities { get; set; }
        public void Link(Base linked)
        {
            ((ObjectX)linked).ObjectType = Const.Object.Task;
        }
    }

    public class TaskAdd : TaskX, IOneToManyAdd
    {
        [JsonIgnore] protected new int Key { get; set; }
        [JsonIgnore] protected new int RelatedKey { get; set; }
        public new List<int> RelatedKeys { get; set; }
    }
}
