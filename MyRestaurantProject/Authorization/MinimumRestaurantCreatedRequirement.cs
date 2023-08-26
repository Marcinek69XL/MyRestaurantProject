using Microsoft.AspNetCore.Authorization;

namespace MyRestaurantProject.Authorization
{
    public class MinimumRestaurantCreatedRequirement : IAuthorizationRequirement
    {
        public int MinimumRestaurantsCreated { get; }
        public MinimumRestaurantCreatedRequirement(int minimumRestaurantsCreated)
        {
            MinimumRestaurantsCreated = minimumRestaurantsCreated;
        }
    }
}