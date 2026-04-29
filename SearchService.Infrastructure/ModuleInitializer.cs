using Microsoft.Extensions.DependencyInjection;
using Q.Commons.ModuleInitializer;
using SearchService.Domain.Repositories;
using SearchService.Infrastructure.Repositories;

namespace SearchService.Infrastructure
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<ISearchRepository, SearchRepository>();
        }
    }
}
