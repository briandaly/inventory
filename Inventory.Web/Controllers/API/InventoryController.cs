using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Inventory.Data.Models;
using Inventory.Data.Repositories;

namespace Inventory.Controllers.API
{
    public class InventoryController : ApiController
    {
        IInventoryRepository _inventoryRepository { get; set; }

        public InventoryController() 
            : this (new InventoryRepository())
        {
            
        }

        public InventoryController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        // GET api/<controller>
        public IEnumerable<InventoryItem> Get()
        {
            return _inventoryRepository.GetAll();
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(int id)
        {
            var item = _inventoryRepository.Get(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // POST api/<controller>
        public IHttpActionResult Post(InventoryItem item)
        {
            if (ModelState.IsValid)
            {
                _inventoryRepository.Add(item);
                _inventoryRepository.SaveChanges();

                return Created<InventoryItem>(Request.RequestUri + item.Id.ToString(), item);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT api/<controller>/5
        public IHttpActionResult Put(int id, InventoryItem item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != item.Id)
            {
                return BadRequest();
            }

            _inventoryRepository.Update(item);
            _inventoryRepository.SaveChanges();

            return Ok();
        }

        // DELETE api/<controller>/5
        public IHttpActionResult Delete(int id)
        {
            InventoryItem item = _inventoryRepository.Get(id);
            if (item == null)
            {
                return NotFound();
            }

            _inventoryRepository.Delete(id);
            _inventoryRepository.SaveChanges();

            NotifyOnDelete(item);

            return Ok();
        }

        // TODO - Move to Service Layer
        private void NotifyOnDelete(InventoryItem item)
        {
            NotificationHub hub = new NotificationHub();
            hub.Send("USER", item.Label + " has been removed.");
        }
    }
}