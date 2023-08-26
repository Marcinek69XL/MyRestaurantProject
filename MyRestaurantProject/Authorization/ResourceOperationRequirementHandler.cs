using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MyRestaurantProject.Entities;

namespace MyRestaurantProject.Authorization
{
    // Z drugim paramertu w AuthorizationHandler <..., tu>, czyli uzywam w serwisach po przez wstrzykniecie
    // IAuthorizationService i skorzystanie z _authorizationService.AuthorizeAsync(...)
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Restaurant>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            ResourceOperationRequirement requirement,
            Restaurant restaurant)
        {
            // Kazdy moze czytac i tworzyc
            if (requirement.ResourceOperation is ResourceOperation.Read or ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }
            
            // Jesli sam ją stworzył, możę wszystko
            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (restaurant.CreatedById == userId)
            {
                context.Succeed(requirement);
            }
            
            return Task.CompletedTask;
        }
    }
}