using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Q.Commons;
using Q.Swagger;
using StackExchange.Redis;

namespace Q.Initializer
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder ConfigureExtraServices(this WebApplicationBuilder builder, InitializerOptions initializerOpt)
        {
            ConfigurationManager configuration = builder.Configuration;
            IServiceCollection services = builder.Services;

            // 注册各模块自己的服务
            var assemblies = ReflectionHelper.GetAllReferencedAssemblies();
            services.InitializeModules(assemblies);

            // 注册Swagger + JWT
            JwtOptions jwtOpt = configuration.GetSection("JwtOptions").Get<JwtOptions>()!;
            services.AddSwagger(jwtOpt, initializerOpt.SwaggerTitle);

            // 注册Redis
            string redisConnStr = configuration.GetValue<string>("Redis:ConnectionString")!;
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnStr);
            services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
            return builder;
        }
    }
}
