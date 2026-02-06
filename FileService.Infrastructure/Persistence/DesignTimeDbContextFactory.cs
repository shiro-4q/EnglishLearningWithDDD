using FileService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using Q.Infrastructure.EFCore;
using Q.Initializer;

namespace FileService.WebAPI;

public class DesignTimeDbContextFactory(IOptionsSnapshot<EFCoreOptions> optionsSnapshot) : IDesignTimeDbContextFactory<FSDbContext>
{
    private readonly EFCoreOptions _eFCoreOptions = optionsSnapshot.Value;

    public FSDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = DbContextOptionsBuilderFactory.Create<FSDbContext>(_eFCoreOptions.ConnectionString);
        return new FSDbContext(optionsBuilder.Options, null);
    }
}