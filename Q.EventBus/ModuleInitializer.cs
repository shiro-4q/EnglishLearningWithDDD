using Microsoft.Extensions.DependencyInjection;
using Q.Commons.ModuleInitializer;

namespace Q.EventBus
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<IEventBus, CapEventBus>();
        }
    }
}
