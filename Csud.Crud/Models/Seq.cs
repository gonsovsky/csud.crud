using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Entities;

namespace Csud.Crud.Models
{
    public class Seq: Entity
    {
        public int Key { get; set; }

        public override string GenerateNewID()
        {
            return GetType().Name;
        }
    }
}
