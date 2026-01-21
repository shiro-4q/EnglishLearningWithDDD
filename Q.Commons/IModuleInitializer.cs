using Microsoft.Extensions.DependencyInjection;

namespace Q.Commons
{
    public interface IModuleInitializer
    {
        public void Initialize(IServiceCollection services);
    }
}
