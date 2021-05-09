using System;
using System.Linq;
using Csud.Crud;
using Csud.Crud.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crud.Csud.RestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class BaseController<T>: ControllerBase where T : Base
    {
        protected static ICsud Csud => CsudService.Csud;

        [HttpGet("list")]
        public virtual IActionResult List(string status=Const.Status.Actual, int skip=0, int take=0)
        {
            try
            {
                var q = Csud.List<T>(status, skip, take);
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
                var entity = Csud.Select<T>().First(a=> a.Key == key);
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

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Post(int key, T entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existing = Csud.Select<T>().First(a => a.Key == key);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.CopyTo(existing, false);
                Csud.Upd(existing);
                return Ok(existing);
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
                Csud.Insert(entity);
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
                Csud.Del(Csud.Select<T>().First(a => a.Key == key));
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
                Csud.Copy(Csud.Select<T>().First(a => a.Key == key));
                return Ok();
            }
            catch (Exception ex)
            {
                return this.Problem(ex.Message);
            }
        }
    }
}
