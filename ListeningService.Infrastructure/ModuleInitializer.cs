using ListeningService.Domain.Interfaces;
using ListeningService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Q.Commons.ModuleInitializer;

namespace ListeningService.Infrastructure
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<ISubtitleHelper, SubtitleHelper>();
            services.AddScoped<ISubtitleParser, LrcParser>();
            services.AddScoped<ISubtitleParser, SrtParser>();
        }
    }
}
