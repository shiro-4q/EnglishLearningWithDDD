using IdentityService.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Q.Commons.ModuleInitializer;

namespace IdentityService.Domain
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<IdDomainService>();
        }
    }
}
