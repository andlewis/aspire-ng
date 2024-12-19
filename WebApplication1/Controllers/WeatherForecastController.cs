using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController(ApiDb db, ILogger<WeatherForecastController> logger) : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet(nameof(GetWeatherForecast))]
        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            var w = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            foreach (var item in w)
            {
                db.Weathers.Add(new Weather
                {
                    Id = Guid.NewGuid(),
                    Value = item.Summary!
                });
            }
            db.SaveChanges();

            return w;
        }
    }
}
