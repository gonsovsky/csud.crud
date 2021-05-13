using System;
using Csud.Crud.Models;
using Microsoft.AspNetCore.Mvc;

namespace Csud.Crud.RestApi.Controllers
{
    [Route("api/context")]
    [ApiController]
    public class RelationalController<TEntity, TModelAdd, TLinked> : Controller where TEntity: Base,IRelational where TModelAdd: Base, IRelational where TLinked : Base
    {
        protected static ICsud Csud => CsudService.Csud;

        [HttpGet("list")]
        public virtual IActionResult List(int skip = 0, int take = 0, string status = Const.Status.Actual)
        {
            try
            {
                var q = Csud.ListRelational<TEntity,TLinked>(0, status, skip, take);
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
                var entity = Csud.GetRelational<TEntity,TLinked>(key);
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
                Csud.DeleteRelational<TEntity>(key);
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
                Csud.CopyRelational<TEntity>(key);
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
        public virtual IActionResult Put(TModelAdd entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var p = Activator.CreateInstance<TEntity>();
                entity.CopyTo(p,false);
                Csud.InsertRelational<TEntity,TLinked>(p);
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return this.Problem(ex.Message);
            }
        }

        [HttpPost("include")]
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
                return Ok();
            }
            catch (Exception ex)
            {
                return this.Problem(ex.Message);
            }
        }

        [HttpPost("exclude")]
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
                return Ok();
            }
            catch (Exception ex)
            {
                return this.Problem(ex.Message);
            }
        }
    }
}
