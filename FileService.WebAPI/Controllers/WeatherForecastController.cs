using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Q.Infrastructure.Cache;
using Q.Infrastructure.EFCore;
using Q.Swagger;
using System.Security.Claims;

namespace FileService.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController(ICache cache, IJwtTokenService jwtTokenService, BaseDbContext dbContext) : ControllerBase
    {
        private readonly ICache _cache = cache;
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
        private readonly BaseDbContext _dbContext = dbContext;

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
            return _dbContext.GetType().ToString();
        }
    }
}
