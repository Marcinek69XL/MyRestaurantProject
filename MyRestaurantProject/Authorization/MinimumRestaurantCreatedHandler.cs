using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MyRestaurantProject.Entities;

namespace MyRestaurantProject.Authorization
{
    //Bez drugiego paramertu w AuthorizationHandler <..., tu>, czyli daje to w Autorize (Policy = tu2)
    public class MinimumRestaurantCreatedHandler :
        AuthorizationHandler<MinimumRestaurantCreatedRequirement>
    {
        private readonly RestaurantDbContext _dbContext;

        public MinimumRestaurantCreatedHandler(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            MinimumRestaurantCreatedRequirement requirement
           )
        {
            var userId = int.Parse(context
                .User
                .FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!
                .Value);
            
            var numberOfRestaurantCreatedByUserIsSufficent = _dbContext
                .Restaurants
                .Count(r => r.CreatedById == userId) >= requirement.MinimumRestaurantsCreated;
            
            if (numberOfRestaurantCreatedByUserIsSufficent)
                context.Succeed(requirement);
            
            return Task.CompletedTask;
        }
    }
}