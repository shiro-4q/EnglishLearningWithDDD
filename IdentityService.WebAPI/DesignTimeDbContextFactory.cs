using IdentityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Design;
using Q.Initializer;

namespace IdentityService.WebAPI;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdDbContext>
{
    public IdDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("Default");

        var optionsBuilder = DbContextOptionsBuilderFactory.Create<IdDbContext>(connectionString!);
        return new IdDbContext(optionsBuilder.Options);
    }
}