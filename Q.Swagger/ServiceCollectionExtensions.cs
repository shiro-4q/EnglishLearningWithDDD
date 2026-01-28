using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Q.Swagger.Jwt;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace Q.Swagger
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Swagger generation and, optionally, JWT authentication support to the specified service collection.
        /// </summary>
        /// <remarks>Call this method during application startup to configure Swagger UI and, optionally,
        /// integrate JWT authentication into the API documentation. If JWT support is enabled, the Swagger UI will be
        /// set up to support authenticated API requests using JWT tokens.</remarks>
        /// <param name="services">The service collection to which Swagger and JWT authentication services will be added.</param>
        /// <param name="jwtOptions">The options used to configure JWT authentication. Required if JWT support is enabled.</param>
        /// <param name="swaggerTitle">The title to display in the generated Swagger documentation.</param>
        /// <param name="enableJwt">true to enable JWT authentication support in Swagger; otherwise, false. The default is true.</param>
        public static void AddSwagger(this IServiceCollection services, JwtOptions jwtOptions, string swaggerTitle, bool enableJwt = true)
        {
            // 基础Swagger支持
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = swaggerTitle, Version = "v1" });
            });

            // 可选的JWT支持
            if (enableJwt)
            {
                // swagger JWT支持
                services.Configure<SwaggerGenOptions>(options =>
                {
                    // 定义JWT Bearer认证方案
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,//http:标准HTTP Authorization, ApiKey 自定义Key
                        In = ParameterLocation.Header,
                        Scheme = "Bearer",//指定HTTP Authorization 的 scheme 名称。实际表现为: Authorization: Bearer {token}
                        Name = "Authorization",
                        BearerFormat = "JWT",// 可选, 指定token格式，没有实际意义
                        Description = "Authorization header. Example: 'Authorization: Bearer 12345abcdef'",
                    });
                    // 将认证方案应用到所有 API，名称需要和上面定义的保持一致
                    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                    });
                });

                services.AddJwt(jwtOptions);
            }
        }

        /// <summary>
        /// Adds JWT bearer authentication to the specified service collection using the provided JWT options.
        /// </summary>
        /// <remarks>This method configures the authentication middleware to validate JWT tokens according
        /// to the specified options. It should be called during application startup as part of service
        /// configuration.</remarks>
        /// <param name="services">The service collection to which the JWT authentication services are added. This parameter cannot be null.</param>
        /// <param name="jwtOptions">The options used to configure JWT authentication, including issuer, audience, and signing key. This
        /// parameter cannot be null.</param>
        public static void AddJwt(this IServiceCollection services, JwtOptions jwtOptions)
        {
            // JWT认证
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
                    };
                });
        }
    }
}
