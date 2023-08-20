using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyRestaurantProject.Entities;
using MyRestaurantProject.Exceptions;
using MyRestaurantProject.Models;
using MyRestaurantProject.Services;

namespace MyRestaurantProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // Dzieki temu mozna sie pozbyc walidacji // sprawdzenie poprawnosci modelu !ModelState.IsValid...
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
            
            return Ok(restaurantDto);
        }

        [HttpPost]
        public ActionResult Post([FromBody] CreateRestaurantDto createDto)
        {
            var newId = _restaurantService.CreateRestaurant(createDto);

            return Created($"api/Restaurant/{newId}", null);
        }
        
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _restaurantService.Delete(id);
         
            return NoContent(); // 204
        }
        
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] UpdateRestaurantDto dto,[FromRoute] int id)
        {
            _restaurantService.UpdateRestaurant(dto, id);
            
            return Ok();
        }
    }
}
