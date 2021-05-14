using System;
using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Services;
using Microsoft.AspNetCore.Mvc;

namespace Csud.Crud.RestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class EntityController<T>: ControllerBase where T : Base
    {
        protected readonly IEntityService<T> Svc;

        protected EntityController(IEntityService<T> svc)
        {
            Svc = svc;
        }

        [HttpGet("list")]
        public virtual IActionResult List(int skip=0, int take=0, string status = Const.Status.Actual)
        {
            try
            {
                var q = Svc.List(status, skip, take);
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
                var entity = Svc.Select().First(a=> a.Key == key);
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
        public virtual IActionResult Post(T entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existing = Svc.Select().First(a => a.Key == entity.Key);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.CopyTo(existing, false);
                Svc.Update(existing);
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
                Svc.Add(entity);
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
                Svc.Delete(Svc.Select().First(a => a.Key == key));
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
                Svc.Copy(Svc.Select().First(a => a.Key == key));
                return Ok();
            }
            catch (Exception ex)
            {
                return this.Problem(ex.Message);
            }
        }
    }
}
