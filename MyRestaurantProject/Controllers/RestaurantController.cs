using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyRestaurantProject.Entities;
using MyRestaurantProject.Models;
using MyRestaurantProject.Services;

namespace MyRestaurantProject.Controllers
{
    [Route("api/{controller}")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IMapper _mapper;

        public RestaurantController(IRestaurantService restaurantService, IMapper mapper)
        {
            _restaurantService = restaurantService;
            _mapper = mapper;
        }

        [HttpGet()]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll()
        {
            var restaurantsDto = _restaurantService.GetAll();
            
            return Ok(restaurantsDto);
        }

        [HttpGet("{id}")]
        public ActionResult<Restaurant> Get([FromRoute]int id)
        {
            var restaurantDto = _restaurantService.Get(id);

            if (restaurantDto is null)
                return NotFound();

            return Ok(restaurantDto);
        }

        [HttpPost]
        public ActionResult Post([FromBody] CreateRestaurantDto createDto)
        {
            // sprawdzenie poprawnosci modelu
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newId = _restaurantService.CreateRestaurant(createDto);

            return Created($"api/Restaurant/{newId}", null);
        }
        
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var isRemoved = _restaurantService.Delete(id);

            if (isRemoved)
                return NoContent();
            else
                return NotFound();
        }
        
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] UpdateRestaurantDto dto,[FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var isRemoved = _restaurantService.UpdateRestaurant(dto, id);

            if (isRemoved)
                return Ok();
            else
                return NotFound();
        }
    }
}
