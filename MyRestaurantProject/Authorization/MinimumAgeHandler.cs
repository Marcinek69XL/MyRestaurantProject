using System;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace MyRestaurantProject.Authorization
{
    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        private readonly ILogger<MinimumAgeRequirement> _logger;

        public MinimumAgeHandler(ILogger<MinimumAgeRequirement> logger)
        {
            _logger = logger;
        }
        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var dateOfBirth = DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth").Value);
            var userEmail = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;
            
            _logger.LogInformation($"User: {userEmail} with date: {dateOfBirth}");
            
            if (dateOfBirth.AddYears(requirement.MinimumAge) < DateTime.Today)
            {
                _logger.LogInformation("Authorization success");
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogInformation("Authorization failed");
            }

            return Task.CompletedTask;
        }
    }
}