using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Q.Commons.ModuleInitializer;
using Q.Infrastructure.EFCore;
using Q.Swagger;
using Q.Swagger.Jwt;
using Serilog;
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
            services.InitializeModules();

            // 注册EFCore的DbContext
            var dbConnStr = configuration.GetValue<string>("ConnectionStrings:Default");
            services.AddAllDbContexts(opt =>
            {
                opt.UseMySql(dbConnStr, ServerVersion.AutoDetect(dbConnStr));
            });

            // 注册身份认证
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            // 注册Swagger + JWT
            //services.Configure<JwtOptions>(configuration.GetSection("JwtOptions")); // 旧的写法，不支持链式调用，不支持Validate
            services.AddOptions<JwtOptions>().Bind(configuration.GetSection("JwtOptions"))
                .Validate(o => !string.IsNullOrEmpty(o.SigningKey), "SigningKey is required")
                .ValidateOnStart();
            JwtOptions jwtOpt = configuration.GetSection("JwtOptions").Get<JwtOptions>()!;
            services.AddSwagger(jwtOpt, initializerOpt.SwaggerTitle);

            // 注册MediatR
            services.AddMediatorR();

            // 注册CORS
            var corsOrigins = configuration.GetSection("Cors:Origins").Get<string[]>() ?? [];
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(corsOrigins)
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });

            // 注册Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()// 添加上下文增强
                .WriteTo.Console()
                //.WriteTo.Console(new JsonFormatter())// 使用JSON 格式
                .WriteTo.File(initializerOpt.LogFilePath)
                .CreateLogger();
            services.AddSerilog();

            // 注册Redis
            string redisConnStr = configuration.GetValue<string>("Redis:ConnectionString")!;
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnStr);
            services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
            return builder;
        }
    }
}
