using ListeningService.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Q.Commons.ModuleInitializer;

namespace ListeningService.Domain
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<ListeningDomainService>();
        }
    }
}
