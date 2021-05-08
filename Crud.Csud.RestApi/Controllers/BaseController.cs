using System;
using System.Linq;
using Crud.Csud.RestApi.Services;
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

        [HttpGet]
        public virtual IActionResult Get(int skip=0, int take=0)
        {
            try
            {
                var q = Csud.Q<T>();
                if (skip != 0)
                    q = q.Skip(skip);
                if (take != 0)
                    q = q.Take(take);
                
                return Ok(q.ToList());

            }
            catch (Exception ex)
            {
                return this.Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public virtual IActionResult Get(int id)
        {
            try
            {
                var entity = Csud.Q<T>().First(a=> a.Key == id);
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
        public virtual IActionResult Post(int id, T entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existing = Csud.Q<T>().First(a => a.Key == id);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.CopyTo(existing, false);
                Csud.UpdateEntity(existing);
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
        public virtual IActionResult Put(int id, T entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Csud.AddEntity(entity);
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return this.Problem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public virtual IActionResult Delete(int id)
        {
            try
            {
                Csud.DelEntity(Csud.Q<T>().First(a => a.Key == id));
                return Ok();
            }
            catch (Exception ex)
            {
                return this.Problem(ex.Message);
            }
        }

        [HttpPut("{id}/copy")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public virtual IActionResult Copy(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Csud.CopyEntity(Csud.Q<T>().First(a => a.Key == id));
                return Ok();
            }
            catch (Exception ex)
            {
                return this.Problem(ex.Message);
            }
        }
    }
}
