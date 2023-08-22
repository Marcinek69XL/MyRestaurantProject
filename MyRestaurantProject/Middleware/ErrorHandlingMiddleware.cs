using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyRestaurantProject.Exceptions;

namespace MyRestaurantProject.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                /* Kontynuacja dalszego przeplywu zadania http,
                dla przykladu gdybym wykomentowal to na moim Middleware przeplyw zapytania
                sie zatrzyma, i np zapytanie GET (i każde inne) sie nie wykona. */
                await next.Invoke(context);
            }
            catch (BadRequestException badRequestException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(badRequestException.Message);
            }
            catch (NotFoundException notFoundException)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync(notFoundException.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong");
            }
        }
    }
}