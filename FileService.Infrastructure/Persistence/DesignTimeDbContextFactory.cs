using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Q.Initializer;

namespace FileService.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FSDbContext>
{
    public FSDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("Default");

        var optionsBuilder = DbContextOptionsBuilderFactory.Create<FSDbContext>(connectionString!);
        return new FSDbContext(optionsBuilder.Options, null);
    }
}