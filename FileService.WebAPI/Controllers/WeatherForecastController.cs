using FileService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Q.EventBus;
using Q.Infrastructure.Cache;
using Q.Swagger.Jwt;
using Serilog;
using System.Security.Claims;

namespace FileService.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController(ICache cache, IJwtTokenService jwtTokenService, FSDbContext dbContext, IEventBus eventBus) : ControllerBase
    {
        private readonly ICache _cache = cache;
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
        private readonly FSDbContext _dbContext = dbContext;
        private readonly IEventBus _eventBus = eventBus;

        [HttpGet]
        public async Task<string> BuildToken()
        {
            List<Claim> claims =
            [
                new Claim(ClaimTypes.Name, "testUser"),
                new Claim(ClaimTypes.Role, "Admin")
            ];
            return _jwtTokenService.BuildToken(claims);
        }

        [HttpGet]
        [Authorize]
        public async Task<string> GetWithAuthorize()
        {
            var value = await _cache.GetOrCreateAsync("testKey", () => Task.FromResult(123));
            List<Claim> claims =
            [
                new Claim(ClaimTypes.Name, "testUser"),
                new Claim(ClaimTypes.Role, "Admin")
            ];
            var value2 = _cache.GetOrCreate("tokenTest", () => "456");
            return value + "," + value2;
        }

        [HttpGet]
        public string GetWithEFCore()
        {
            return "数量：" + _dbContext.Persons.Count();
        }

        [HttpGet]
        public string GetWithLogger()
        {
            Log.Information("Starting web application");
            Log.Information("123");
            Log.Warning("你好 {user} 我{age}", "qw", 14);
            return "success";
        }

        [HttpGet]
        public string Publish()
        {
            _eventBus.Publish("test.event", new { Name = "Test Event", Timestamp = DateTime.UtcNow });
            return "success";
        }
    }
}
