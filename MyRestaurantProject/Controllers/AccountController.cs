
using Microsoft.AspNetCore.Mvc;
using MyRestaurantProject.Models;
using MyRestaurantProject.Services;

namespace MyRestaurantProject.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService _accountService)
        {
            this._accountService = _accountService;
        }
        
        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterUserDto dto)
        {
            var userId = _accountService.RegisterUser(dto);
            return Ok($"New user id: {userId}");
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginUserDto dto)
        {
            var token = _accountService.GenerateJWT(dto);
            return Ok(token);
        }
        
    }
}