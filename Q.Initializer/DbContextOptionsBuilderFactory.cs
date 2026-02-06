using Microsoft.EntityFrameworkCore;

namespace Q.Initializer
{
    public static class DbContextOptionsBuilderFactory
    {
        public static DbContextOptionsBuilder<TDbContext> Create<TDbContext>(string connectionString)
            where TDbContext : DbContext
        {
            var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            //optionsBuilder.UseSqlServer(connStr);
            return optionsBuilder;
        }
    }
}
