using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Caching.Distributed;

using BeautyManager.Web.Data;
using BeautyManager.Web.Repository.MongoDB;
using BeautyManager.Web.Repository.Settings;
using BeautyManager.Web.Repository;

namespace BeautyManager.Web.Api.Controllers
{
    public class UsersController : ODataController
    {
        IRepository<User> _repository;

        public UsersController(IRepository<User> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IQueryable<User>> Get()
        {
            var items = await _repository.GetItemsAsync();
            return items.AsQueryable();
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get(string key)
        {
            return Ok(await _repository.GetItemAsync(key));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]User entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!string.IsNullOrEmpty(entity.UserId) && string.IsNullOrEmpty(entity.Id))
			{
                entity.Id = entity.UserId;
            }
            entity.CreatedOn = DateTime.Now;
            entity.UpdatedOn = entity.CreatedOn;
            var addedEntity = await _repository.AddItemAsync(entity);
            if (string.IsNullOrEmpty(addedEntity.UserId))
			{
                addedEntity.UserId = addedEntity.Id;
                addedEntity = await _repository.UpdateItemAsync(addedEntity.Id, addedEntity);
            }
            return Created(addedEntity);
        }

       
        [HttpPut]
        public async Task<IActionResult> Put(string key, [FromBody] User userUpdate)
		{
            if (!ModelState.IsValid)
			{
                return BadRequest(ModelState);
			}
            if (userUpdate == null)
            {
                return NotFound();
            }
            if (key != userUpdate.UserId)
			{
                return BadRequest();
			}
            var entity = await _repository.GetItemAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            if (entity.UpdatedOn != userUpdate.UpdatedOn)
            {
                return Conflict();
            }
            entity.UpdatedOn = DateTime.Now; 
            var updatedEntity = await _repository.UpdateItemAsync(key, userUpdate);
            return Updated(updatedEntity);
        }

        [HttpPatch]
        public async Task<IActionResult> Patch(string key, [FromBody] Delta<User> userPatch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await _repository.GetItemAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            DateTime dbUpdatedOn = entity.UpdatedOn;
            userPatch.Patch(entity);
            if (entity.UpdatedOn != dbUpdatedOn)
            {
                return Conflict();
            }
            entity.UpdatedOn = DateTime.Now;
            var updatedEntity = await _repository.UpdateItemAsync(key, entity);
            return Updated(entity);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string key)
		{
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key == null)
            {
                return BadRequest();
            }

            await _repository.DeleteItemAsync(key);
            return Ok();
        }

    }
}
