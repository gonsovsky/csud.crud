using System;
using System.Linq;
using Crud.Csud.RestApi.Models;
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
        public virtual IActionResult List(string status = Const.Status.Actual, int skip =0, int take=0)
        {
            try
            {
                var q = Csud.ListContext(status, skip, take);
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

        [HttpDelete("{key}")]
        public virtual IActionResult Delete(int key)
        {
            try
            {
                Csud.DelContext(key);
                return Ok();
            }
            catch (Exception ex)
            {
                return this.Problem(ex.Message);
            }
        }

        [HttpPost("{key}/copy")]
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
                Csud.CopyContext(key);
                return Ok();
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
        protected virtual IActionResult Put<T>(bool isTemporary, T entity) where T: BaseContext
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Csud.AddContext<T>(entity,isTemporary);
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
            => Put<TimeContext>(isTemporary, (TimeContext)entity);

        [HttpPut("rule")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Put(bool isTemporary, RuleContext entity)
            => Put<RuleContext>(isTemporary, (RuleContext)entity);

        [HttpPut("segment")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Put(bool isTemporary, SegmentContext entity)
            => Put<SegmentContext>(isTemporary, (SegmentContext)entity);

        [HttpPut("struct")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Put(bool isTemporary, StructContext entity)
            => Put<StructContext>(isTemporary, (StructContext)entity);

        [HttpPut("composite")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Put(bool isTemporary, CompositeContextModel entity)
        {
            var c = new CompositeContext();
            entity.CopyTo(c, false);
            if (entity.RelatedKeys==null || entity.RelatedKeys.Length==0)
                throw new ArgumentException($"Связанные контексты не найдены");
            foreach (var x in entity.RelatedKeys)
            {
                if (Csud.Context.Any(a => a.Key == x) == false)
                    throw new ArgumentException($"Контекст с кодом {x} не найден");
                c.Compose(x);  
            }
            return Put<CompositeContext>(isTemporary, (CompositeContext)c);
        }
    }
}
