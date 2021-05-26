using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Csud.Crud.Models.Rules;
using Csud.Crud.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations.Rules;

namespace Csud.Crud.RestApi.Controllers.customized
{
    public class Id
    {
        [JsonPropertyName("key")]
        [Description("description 1")]
        public string Key { get; set; }

        [JsonPropertyName("provider")]
        [Description("description 2")]
        public int Provider { get; set; }
    }

    [Route("api/accountex")]
    [ApiController]
    public class AccountExController : OneController<Account, AccountAdd, AccountEdit>
    {
        public AccountExController(IEntityService<Account> svc) : base(svc)
        {
            
        }

        [HttpGet("A")]
        public  IActionResult Get([FromQuery] Id id)
        {
            return base.Get(1);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("somewhere)")]
        public override IActionResult Get(int key)
        {
            return base.Get(key);
        }

    }
}
