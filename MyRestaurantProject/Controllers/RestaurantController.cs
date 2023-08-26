using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
  //  [Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
//        [Authorize] - mozna na poziomie metody
//        [Authorize(Policy = "HasNationality")] // jesli nie spelnia, 403 Forbidden
        [Authorize(Policy = "Atleast2RestaurantCreated")]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll()
        {
            var restaurantsDto = _restaurantService.GetAll();
            
            return Ok(restaurantsDto);
        }
        
        [HttpGet("{id}")]
        // [AllowAnonymous] // - wylacza autoryzacje
        [Authorize(Policy = "Atleast20")]
        public ActionResult<Restaurant> Get([FromRoute]int id)
        {
            var restaurantDto = _restaurantService.Get(id);
            
            return Ok(restaurantDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")] // gdy nie mamy danej roli, rzuca 403 Forbidden
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto createDto)
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
