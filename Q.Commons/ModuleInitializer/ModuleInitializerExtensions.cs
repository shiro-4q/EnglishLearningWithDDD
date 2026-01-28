using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Q.Commons.ModuleInitializer
{
    public static class ModuleInitializerExtensions
    {
        public static IServiceCollection InitializeModules(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            var moduleInitializerTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IModuleInitializer).IsAssignableFrom(type) && !type.IsAbstract);
            foreach (var type in moduleInitializerTypes)
            {
                var initializer = (IModuleInitializer)Activator.CreateInstance(type)! ?? throw new ApplicationException($"Cannot create ${type}"); ;
                initializer.Initialize(services);
            }
            return services;
        }

        public static IServiceCollection InitializeModules(this IServiceCollection services)
        {
            var moduleInitializerTypes = AppDomain.CurrentDomain.GetAssemblies()
                 .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IModuleInitializer).IsAssignableFrom(type) && !type.IsAbstract);
            foreach (var type in moduleInitializerTypes)
            {
                var initializer = (IModuleInitializer)Activator.CreateInstance(type)! ?? throw new ApplicationException($"Cannot create ${type}"); ;
                initializer.Initialize(services);
            }
            return services;
        }
    }
}
