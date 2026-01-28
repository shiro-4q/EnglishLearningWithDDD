using Microsoft.Extensions.DependencyInjection;
using Q.Commons.ModuleInitializer;
using Q.Swagger.Jwt;

namespace Q.Swagger
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddSingleton<IJwtTokenService, JwtTokenService>();
        }
    }
}
