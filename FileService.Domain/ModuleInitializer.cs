using FileService.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Q.Commons.ModuleInitializer;

namespace FileService.Domain
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<FSDomainService>();
        }
    }
}
