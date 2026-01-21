using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Q.Commons;
using Q.Infrastructure.Cache;
using Q.Swagger;
using StackExchange.Redis;
using System.Reflection;

namespace Q.Initializer
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder ConfigureExtraServices(this WebApplicationBuilder builder, InitializerOptions initializerOpt)
        {
            ConfigurationManager configuration = builder.Configuration;
            IServiceCollection services = builder.Services;

            // 注册Swagger + JWT
            JwtOptions jwtOpt = configuration.GetSection("JwtOptions").Get<JwtOptions>()!;
            services.AddSwagger(jwtOpt, initializerOpt.SwaggerTitle);

            // 注册Redis
            string redisConnStr = configuration.GetValue<string>("Redis:ConnectionString")!;
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnStr);
            services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
            services.AddSingleton<ICache, RedisCache>();

            var assembly = Assembly.GetEntryAssembly();
            var assembly2 = Assembly.GetExecutingAssembly();
            var list1 = assembly.GetReferencedAssemblies();
            var list2 = assembly2.GetReferencedAssemblies();
            var types1 = assembly.GetTypes();
            var types2 = assembly2.GetTypes();
            List<Assembly> assemblies = [];
            foreach (var item in list1)
            {

                if (item.FullName.Contains("Q."))
                {
                    var itemAssembly = Assembly.Load(item);
                    assemblies.Add(itemAssembly);
                }
            }
            List<Assembly> assemblies2 = [];
            foreach (var item in list2)
            {

                if (item.FullName.Contains("Q."))
                {
                    var itemAssembly = Assembly.Load(item);
                    assemblies2.Add(itemAssembly);
                }
            }
            List<Type> typeList = [];
            foreach (var item in assemblies2)
            {
                var types = item.GetTypes().Where(x => !x.IsAbstract && typeof(IModuleInitializer).IsAssignableFrom(x));
                typeList.AddRange(types);
            }
            return builder;
        }
    }
}
