using Microsoft.AspNetCore.Mvc;
using Q.Infrastructure.Cache;

namespace FileService.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(ICache cache) : ControllerBase
    {
        private readonly ICache _cache = cache;

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<string> Get()
        {
            var value = await _cache.GetOrCreateAsync("testKey", () => Task.FromResult(123));
            var value2 = _cache.GetOrCreate("testKey2", () => 456);
            return value + "," + value2;
        }
    }
}
