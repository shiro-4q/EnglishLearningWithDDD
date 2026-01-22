using Microsoft.Extensions.DependencyInjection;

namespace Q.Commons
{
    /// <summary>
    /// 每个模块注册自己的服务，只需要每个模块实现此接口，并且在实现中注册自己的服务即可
    /// </summary>
    public interface IModuleInitializer
    {
        public void Initialize(IServiceCollection services);
    }
}
