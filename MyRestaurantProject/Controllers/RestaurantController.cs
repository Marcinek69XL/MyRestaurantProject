using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyRestaurantProject.Entities;

namespace MyRestaurantProject.Controllers
{
    [Route("api/{controller}")]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantDbContext _dbContext;

        public RestaurantController(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet()]
        public ActionResult<IEnumerable<Restaurant>> GetAll()
        {
            var restaurants =_dbContext
                .Restaurants
                .ToList();

            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public ActionResult<Restaurant> Get([FromRoute]int id)
        {
            var restaurants = _dbContext
                .Restaurants
                .FirstOrDefault(x => x.Id == id);

            if (restaurants is null)
                return NotFound();

            return Ok(restaurants);
        }
    }
}
