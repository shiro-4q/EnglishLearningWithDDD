using IdentityService.Domain.Interfaces;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Repositories;
using IdentityService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Q.Commons.ModuleInitializer;

namespace IdentityService.Infrastructure
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<ISmsSender, MockSmsSender>();
            services.AddScoped<IEmailSender, MockEmailSender>();
            services.AddScoped<IIdRepository, IdRepository>();
        }
    }
}
