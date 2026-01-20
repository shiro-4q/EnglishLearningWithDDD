using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using QW.Swagger;

namespace QW.Initializer
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder ConfigureExtraServices(this WebApplicationBuilder builder, InitializerOptions initializerOpt)
        {
            JwtOptions jwtOpt = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>()!;
            builder.Services.AddSwagger(jwtOpt, initializerOpt.SwaggerTitle);
            return builder;
        }
    }
}
