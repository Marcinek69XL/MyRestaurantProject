using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MyRestaurantProject.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private readonly ILogger<RequestTimeMiddleware> _logger;
        private readonly float _maxDurationInMilliseconds;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _logger = logger;
            _maxDurationInMilliseconds = 4000;
        }
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var sw = new Stopwatch();

            sw.Start();
            await next.Invoke(context);
            sw.Stop();
            
            if (sw.ElapsedMilliseconds > _maxDurationInMilliseconds)
                _logger.LogWarning($"Request {context.Request.Method} at {context.Request.Path} execution time exceeded {_maxDurationInMilliseconds/1000.0} seconds, request execution time: {sw.ElapsedMilliseconds/1000.0} seconds");
        }
    }
}