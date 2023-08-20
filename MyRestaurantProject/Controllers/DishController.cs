using Microsoft.AspNetCore.Mvc;
using MyRestaurantProject.Models;
using MyRestaurantProject.Services;

namespace MyRestaurantProject.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }
        
        [HttpPost]
        public ActionResult Post([FromRoute] int restaurantId,[FromBody] CreateDishDto dto)
        {
            var newId = _dishService.Create(restaurantId, dto);

            return Created($"api/{restaurantId}/dish/{newId}", null);
        }
        
        [HttpGet("{dishId}")]
        public ActionResult GetOne([FromRoute] int dishId, [FromRoute] int restaurantId)
        {
            var dishDto = _dishService.GetDish(restaurantId, dishId);

            return Ok(dishDto);
        }
        
        [HttpGet]
        public ActionResult GetAll([FromRoute] int restaurantId)
        {
            var dishDto = _dishService.GetDishes(restaurantId);

            return Ok(dishDto);
        }

        [HttpDelete("{dishId}")]
        public ActionResult DeleteOne([FromRoute] int dishId,[FromRoute] int restaurantId)
        {
            _dishService.DeleteDish(restaurantId, dishId);
            return NoContent();
        }
        
        [HttpDelete]
        public ActionResult DeleteAll([FromRoute] int restaurantId)
        {
            _dishService.DeleteDishes(restaurantId);
            return NoContent();
        }
    }
}