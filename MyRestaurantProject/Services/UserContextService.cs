using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MyRestaurantProject.Services
{
    public interface IUserContextService
    {
        ClaimsPrincipal User { get; }
        int? GetUserId { get; }
    }

    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _accessor;

        public UserContextService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public ClaimsPrincipal User => _accessor.HttpContext?.User;
        public int? GetUserId => User is null ? null :
            int.Parse(User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value);
    }
}