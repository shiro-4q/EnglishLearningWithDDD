using ListeningService.Domain.Interfaces;
using ListeningService.Domain.Repositories;
using ListeningService.Infrastructure.Repositories;
using ListeningService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Q.Commons.ModuleInitializer;

namespace ListeningService.Infrastructure
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<IListeningRepository, ListeningRepository>();
            services.AddScoped<ISubtitleHelper, SubtitleHelper>();
            services.AddScoped<ISubtitleParser, LrcParser>();
            services.AddScoped<ISubtitleParser, SrtParser>();
        }
    }
}
