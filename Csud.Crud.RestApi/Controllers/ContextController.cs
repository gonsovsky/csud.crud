using System;
using Csud.Crud.Models;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Services;
using Microsoft.AspNetCore.Mvc;

namespace Csud.Crud.RestApi.Controllers
{
    [Route("api/context")]
    [ApiController]
    public class ContextController: Controller
    {
        protected IContextService Svc;
        public ContextController(IContextService svc)
        {
            Svc = svc;
        }

        [HttpGet("list")]
        public virtual IActionResult List(string type, int skip = 0, int take = 0, string status = Const.Status.Actual)
        {
            try
            {
                var q = Svc.List(type, status, skip, take);
                return Ok(q);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{key}")]
        public virtual IActionResult Get(int key)
        {
            try
            {
                var entity = Svc.Get(key);
                if (entity == null)
                {
                    return NotFound();
                }
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{key}")]
        public virtual IActionResult Delete(int key)
        {
            try
            {
                Svc.Delete(key);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("copy/{key}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Copy(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = Svc.Copy(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPut]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        protected virtual IActionResult Put<T>(T entity) where T: BaseContext
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = Svc.Add(entity);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("time")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Put(TimeContextAdd entity)
            => Put<TimeContextAdd>(entity);

        [HttpPut("rule")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Put(RuleContextAdd entity)
            => Put<RuleContextAdd>(entity);

        [HttpPut("segment")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Put(SegmentContextAdd entity)
            => Put<SegmentContextAdd>(entity);

        [HttpPut("struct")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Put(StructContextAdd entity)
            => Put<StructContextAdd>(entity);

        [HttpPut("attribute")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Put(AttributeContextAdd entity)
            => Put<AttributeContextAdd>(entity);

        [HttpPut("composite")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Put(CompositeContextAdd entity)
            => Put<CompositeContextAdd>(entity);

       

        [HttpPost("composite/include/{key}/{relatedKey}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Include(int key, int relatedKey)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = Svc.Include(key, relatedKey);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("composite/exclude/{key}/{relatedKey}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Exclude(int key, int relatedKey)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = Svc.Exclude(key, relatedKey);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
