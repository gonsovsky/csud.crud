using System;
using System.Linq;
using Crud.Csud.RestApi.Models;
using Csud.Crud;
using Csud.Crud.Models;
using Microsoft.AspNetCore.Mvc;
using Csud.Crud.Models.Contexts;
using Csud.Crud.Models.Rules;

namespace Crud.Csud.RestApi.Controllers
{
    [Route("api/context")]
    [ApiController]
    public class RelationalController<TGroup, TAdd, TEntity> : Controller where TGroup: Base,IRelational where TAdd: Base, IRelational where TEntity : Base
    {
        protected static ICsud Csud => CsudService.Csud;

        [HttpGet("list")]
        public virtual IActionResult List(int skip = 0, int take = 0, string status = Const.Status.Actual)
        {
            try
            {
                var q = Csud.ListRelational<TGroup,TEntity>(0, status, skip, take);
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
                var entity = Csud.GetRelational<TGroup,TEntity>(key);
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
                Csud.DeleteRelational<TGroup>(key);
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
                Csud.CopyRelational<TGroup>(key);
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
        public virtual IActionResult Put(TAdd entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var p = Activator.CreateInstance<TGroup>();
                entity.CopyTo(p,false);
                Csud.InsertRelational<TGroup,TEntity>(p);
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
