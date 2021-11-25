using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestExercise6.Managers;
using RestExercise6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestExercise6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        //A object of the IItemsManager interface
        //Removed the initialization here, as it needs the context from the Constructor
        private IItemsManager _manager;

        //Added this constructor to be able to use the ItemContext in the Manager
        public ItemsController(ItemContext context)
        {
            //an if statement could be implemented here, telling the Service which Manager to use
            _manager = new ItemsManagerDB(context);
        }

        // GET: api/Items
        // GET: api/Items?substring=book&minimumQuality=1&minimumQuantity=1
        //Gets all the items in the managers list
        //Is able to filter the result by either:
        //Name containing the substring (case-insensitive)
        //Quality is more or equal to minimumQuality
        //Quantity is more or equal to minimumQuantity
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        public ActionResult<IEnumerable<Item>> Get([FromQuery] string substring, [FromQuery] int minimumQuality, [FromQuery] int minimumQuantity)
        {
            IEnumerable<Item> result = _manager.GetAll(substring, minimumQuality, minimumQuantity);
            if (result.Count() == 0) return NoContent();
            return Ok(result);
        }

        // GET: api/Items/quality?minQuality=1&maxQuality=10
        //Gets all the items in the managers list
        //Is able to filter the result by either:
        //Name containing the substring (case-insensitive)
        //Quality is more or equal to minimumQuality
        //Quantity is more or equal to minimumQuantity
        //Changed route so the webserver knows which funtion to execute
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet("quality")]
        public ActionResult<IEnumerable<Item>> Get([FromQuery] int minQuality, [FromQuery] int maxQuality)
        {
            IEnumerable<Item> result = _manager.GetAllBetweenQuality(minQuality, maxQuality);
            if (result.Count() == 0) return NoContent();
            return Ok(result);
        }

        // GET api/Items/5
        //Gets a specific Item in the managers list, return null if the object wasn't found
        //Notice the "{id}" part of the annotation, this makes the URI to the object expect a /int
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Item> Get(int id)
        {
            Item result = _manager.GetById(id);
            if (result == null) return NotFound("No such item, id: " + id);
            return Ok(result);
        }

        // POST api/Items
        //Sends the new object to list, which gives it a new ID and return the newly created object
        //Notice the FromBody part of the parameter, this expects a Json body from the request
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        public ActionResult<Item> Post([FromBody] Item newItem)
        {
            Item result = _manager.Add(newItem);
            return Created($"/api/Items/{result.Id}", result);
        }

        // PUT api/Items/5
        //Sends the id and the Item object to the manager to update the values
        //The id in the object is ignored
        //Returns null if no items has the id
        //As seen here, we can mix different ways of expected data from the client
        //Notice the "{id}" part of the annotation, this makes the URI to the object expect a /int
        //Notice the FromBody part of the parameter, this expects a Json body from the request
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public ActionResult<Item> Put(int id, [FromBody] Item updatedItem)
        {
            Item result = _manager.Update(id, updatedItem);
            if (result == null) return NotFound("No such item, id: " + id);
            return Ok(result);
        }

        // DELETE api/Items/5
        //Asks the Manager to delete the item with the specific id
        //Returns null if the item was not found
        //Notice the "{id}" part of the annotation, this makes the URI to the object expect a /int
        //Here we specify that it is only requests coming from https://zealand.dk that is allowed
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EnableCors(Startup.AllowOnlyZealandOriginPolicyName)]
        [HttpDelete("{id}")]
        public ActionResult<Item> Delete(int id)
        {
            Item result = _manager.Delete(id);
            if (result == null) return NotFound("No such item, id: " + id);
            return Ok(result);
        }
    }
}
