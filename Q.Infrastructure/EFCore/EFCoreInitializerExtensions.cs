using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Q.Infrastructure.EFCore
{
    public static class EFCoreInitializerExtensions
    {
        public static IServiceCollection AddAllDbContexts(this IServiceCollection services, Action<DbContextOptionsBuilder>? optionsAction, IEnumerable<Assembly> assemblies)
        {
            // 获取所有继承自DbContext的类型
            var dbContextTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t =>
                    t is not null &&
                    !t.IsAbstract &&
                    typeof(DbContext).IsAssignableFrom(t)).ToList();

            // 获取AddDbContext方法信息
            Type[] types = [typeof(IServiceCollection), typeof(Action<DbContextOptionsBuilder>), typeof(ServiceLifetime), typeof(ServiceLifetime)];
            var addDbContextMethod = typeof(EntityFrameworkServiceCollectionExtensions)
                .GetMethod(nameof(EntityFrameworkServiceCollectionExtensions.AddDbContext), 1, types) ?? throw new ApplicationException("can not found AddDbContext Method");
            foreach (var dbContextType in dbContextTypes)
            {
                // 调用AddDbContext<TContext>方法
                addDbContextMethod.MakeGenericMethod(dbContextType)
                    .Invoke(null, [services, optionsAction, ServiceLifetime.Scoped, ServiceLifetime.Scoped]);// 通过反射调用方法，默认赋值的参数也需要传入
            }
            return services;
        }
    }
}
