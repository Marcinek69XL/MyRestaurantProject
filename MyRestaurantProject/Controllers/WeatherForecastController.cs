using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRestaurantProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastService _service;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger, IWeatherForecastService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        public ActionResult<string> Hello([FromBody] string name)
        {
            return NotFound(name);
        }

        [HttpPost("generate")]
        public ActionResult<IEnumerable<WeatherForecast>> Generate
            ([FromQuery] int numbersOfElements, [FromBody] TempRangeDto dto)
        {
            if (numbersOfElements < 1 || dto.MinValue > dto.MaxValue)
                return BadRequest();

            var result = 
                _service.Generate(numbersOfElements, dto.MinValue, dto.MaxValue);

            return Ok(result);
        }
    }
}
