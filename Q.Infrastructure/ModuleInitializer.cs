using Microsoft.Extensions.DependencyInjection;
using Q.Commons.ModuleInitializer;
using Q.Infrastructure.Cache;

namespace Q.Infrastructure
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddSingleton<ICache, RedisCache>();
        }
    }
}
