using System;
using System.Collections.Generic;
using System.Linq;

namespace MyRestaurantProject
{
    public interface IWeatherForecastService
    {
        public IEnumerable<WeatherForecast> Get();
        IEnumerable<WeatherForecast> Generate(int numbersOfElements, int dtoMinValue, int dtoMaxValue);
    }
    public class WeatherForecastService : IWeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public IEnumerable<WeatherForecast> Generate(int numbersOfElements, int minTempValue, int maxTempValue)
        {
            var rng = new Random();
            return Enumerable.Range(1, numbersOfElements).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(minTempValue, maxTempValue),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }

        public IEnumerable<WeatherForecast> Get()
        {
            throw new NotImplementedException();
        }
    }
}
