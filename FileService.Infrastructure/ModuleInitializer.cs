using FileService.Domain.Interfaces;
using FileService.Domain.Repositories;
using FileService.Infrastructure.Repositories;
using FileService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Q.Commons.ModuleInitializer;

namespace FileService.Infrastructure
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<IStorageClient, MockCloudStorageClient>();
            services.AddScoped<IStorageClient, LocalStorageClient>();
            services.AddScoped<IFSRepository, FSRepository>();
        }
    }
}
