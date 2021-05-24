using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Csud.Crud.Models;
using Csud.Crud.Services;
using Microsoft.AspNetCore.Mvc;

namespace Csud.Crud.RestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OneController<TEntity, TModelAdd, TModelEdit> : Controller 
        where TEntity : Base
        where TModelAdd : TEntity, IAddable
        where TModelEdit : TEntity, IEditable
    {
        protected readonly IEntityService<TEntity> Svc;

        public OneController(IEntityService<TEntity> svc)
        {
            Svc = svc;
        }

        [HttpGet("list")]
        public virtual IActionResult List(int skip = 0, int take = 0, string status = Const.Status.Actual)
        {
            try
            {
                var list = Svc.List(status, skip, take);
                return Ok(list);

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
                var entity = Svc.Look(key);
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

        [HttpPost("{key}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Post(int key, TModelEdit entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                entity.Key = key;
                var result = Svc.Update(entity);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
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
                var result = Svc.Add(entity);
                return Ok(result);
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
                Svc.Delete(Svc.Select().First(a => a.Key == key));
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
                var result = Svc.Copy(Svc.Select().First(a => a.Key == key));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}