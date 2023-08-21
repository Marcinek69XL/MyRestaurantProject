
using Microsoft.AspNetCore.Mvc;
using MyRestaurantProject.Models;
using MyRestaurantProject.Services;

namespace MyRestaurantProject.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountRegisterController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountRegisterController(IAccountService _accountService)
        {
            this._accountService = _accountService;
        }
        
        [HttpPost]
        public ActionResult Register(RegisterUserDto dto)
        {
            var userId = _accountService.RegisterUser(dto);
            return Ok($"New user id: {userId}");
        }
        
    }
}