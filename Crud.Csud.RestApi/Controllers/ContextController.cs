using System;
using System.Linq;
using Crud.Csud.RestApi.Services;
using Csud.Crud;
using Csud.Crud.Models;
using Microsoft.AspNetCore.Mvc;
using Csud.Crud.Models.Contexts;

namespace Crud.Csud.RestApi.Controllers
{
    [Route("api/context")]
    [ApiController]
    public class ContextController: Controller
    {
        protected static ICsud Csud => CsudService.Csud;

        [HttpGet("list")]
        public virtual IActionResult List(int skip=0, int take=0)
        {
            try
            {
                var q = Csud.ListContext(skip, take);
                return Ok(q);
            }
            catch (Exception ex)
            {
                return this.Problem(ex.Message);
            }
        }

        [HttpGet("{key}")]
        public virtual IActionResult Get(int key)
        {
            try
            {
                var entity = Csud.GetContext(key);
                if (entity == null)
                {
                    return NotFound();
                }
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return this.Problem(ex.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        protected virtual IActionResult Put(bool isTemporary, BaseContext entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Csud.AddContext(entity,isTemporary);
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return this.Problem(ex.Message);
            }
        }

        [HttpPut("time")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Put(bool isTemporary, TimeContext entity)
            => Put(isTemporary, (BaseContext)entity);

        [HttpPut("rule")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Put(bool isTemporary, RuleContext entity)
            => Put(isTemporary, (BaseContext)entity);

        [HttpPut("segment")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Put(bool isTemporary, SegmentContext entity)
            => Put(isTemporary, (BaseContext)entity);

        [HttpPut("struct")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Put(bool isTemporary, StructContext entity)
            => Put(isTemporary, (BaseContext)entity);

        [HttpPut("composite")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Put(bool isTemporary, CompositeContext entity)
            => Put(isTemporary, (BaseContext)entity);
    }
}
