using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Q.Initializer
{
    public static class WebApplicationExtensions
    {
        public static WebApplication UseExtraMiddleware(this WebApplication app, InitializerOptions initializerOptions)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("v1/swagger.json", initializerOptions.SwaggerTitle);
                });
            }
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}
