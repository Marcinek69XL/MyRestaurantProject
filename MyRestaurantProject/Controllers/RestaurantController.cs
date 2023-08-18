using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRestaurantProject.Entities;
using MyRestaurantProject.Models;

namespace MyRestaurantProject.Controllers
{
    [Route("api/{controller}")]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;

        public RestaurantController(RestaurantDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet()]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll()
        {
            var restaurants =_dbContext
                .Restaurants
                .Include(x => x.Address)
                .Include(x => x.Dishes)
                .ToList();

            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);

            return Ok(restaurantsDto);
        }

        [HttpGet("{id}")]
        public ActionResult<Restaurant> Get([FromRoute]int id)
        {
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(x => x.Id == id);

            if (restaurant is null)
                return NotFound();

            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            return Ok(restaurantDto);
        }
    }
}
