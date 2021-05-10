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
    public class RelationController<T> : Controller where T: Base,IRelatable
    {
        protected static ICsud Csud => CsudService.Csud;

        [HttpGet("list")]
        public virtual IActionResult List(string status = Const.Status.Actual, int skip = 0, int take = 0)
        {
            try
            {
                var q = Csud.ListRelatable<T>(status, skip, take);
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
                var entity = Csud.GetRelatable<T>(key);
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
                Csud.DeleteRelatable<T>(Csud.Select<T>().First(a => a.Key == key));
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
                Csud.CopyRelatable<T>(Csud.Select<T>().First(a => a.Key == key));
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
        public virtual IActionResult Put(T entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Csud.InsertRelatable<T>(entity);
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
