using MediaEncoderService.Domain.Transcoder;
using Microsoft.Extensions.DependencyInjection;
using Q.Commons.ModuleInitializer;

namespace MediaEncoderService.Domain
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<TranscoderFactory>();
        }
    }
}
