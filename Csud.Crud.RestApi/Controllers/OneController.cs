using System;
using System.Linq;
using Csud.Crud.Models;
using Csud.Crud.Models.Internal;
using Csud.Crud.Services;
using Microsoft.AspNetCore.Mvc;

namespace Csud.Crud.RestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OneController<TEntity, TModelAdd, TModelEdit, TEntityKey> : Controller 
        where TEntity : Base
        where TModelAdd : TEntity, IAddable
        where TModelEdit : TEntity, IEditable
        where TEntityKey : IEntityKey
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

        [HttpGet()]
        public virtual IActionResult Get([FromQuery] TEntityKey key)
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

        [HttpPost()]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Post([FromQuery] TEntityKey key, TModelEdit entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                key.CopyTo(entity);
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

        [HttpDelete]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Delete([FromQuery] TEntityKey key)
        {
            try
            {
                Svc.Delete(Svc.Get(key));
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("copy")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Copy([FromQuery] TEntityKey key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = Svc.Copy(Svc.Get(key));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}