using System;
using System.Linq;
using Csud.Crud.Models;
using Microsoft.AspNetCore.Mvc;
using Csud.Crud.Models.Contexts;

namespace Crud.Csud.RestApi.Controllers
{
    [Route("api/context")]
    [ApiController]
    public class ContextController: BaseController<Context>
    {
        [HttpGet()]
        public override IActionResult Get(int skip, int take)
        {
            try
            {
                var q = Csud.GetContext(skip, take);
                return Ok(q);
            }
            catch (Exception ex)
            {
                return this.Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public override IActionResult Get(int id)
        {
            try
            {
                var entity = Csud.GetContext(id);
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
        protected virtual IActionResult Put(int id, bool isTemporary, BaseContext entity)
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
        public virtual IActionResult Put(int id, bool isTemporary, TimeContext entity)
            => Put(id, isTemporary, (BaseContext)entity);

        [HttpPut("rule")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Put(int id, bool isTemporary, RuleContext entity)
            => Put(id, isTemporary, (BaseContext)entity);

        [HttpPut("segment")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Put(int id, bool isTemporary, SegmentContext entity)
            => Put(id, isTemporary, (BaseContext)entity);

        [HttpPut("struct")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Put(int id, bool isTemporary, StructContext entity)
            => Put(id, isTemporary, (BaseContext)entity);

        [HttpPut("composite")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Put(int id, bool isTemporary, CompositeContext entity)
            => Put(id, isTemporary, (BaseContext)entity);
    }
}
