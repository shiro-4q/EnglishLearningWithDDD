using MediaEncoder.Infrastructure;
using MediaEncoderService.Domain.Repositories;
using MediaEncoderService.Domain.Transcoder;
using MediaEncoderService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Q.Commons.ModuleInitializer;

namespace MediaEncoderService.Infrastructure
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<ITranscodingRepository, TranscodingRepository>();
            services.AddScoped<ITranscoder, ToM4ATranscoder>();
        }
    }
}
