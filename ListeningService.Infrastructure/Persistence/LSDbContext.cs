using MediatR;
using Microsoft.EntityFrameworkCore;
using Q.Infrastructure.EFCore;
using System.Reflection;

namespace ListeningService.Infrastructure.Persistence
{
    public class LSDbContext(DbContextOptions<LSDbContext> options, IMediator? mediator) : BaseDbContext(options, mediator)
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Episode> Episodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.EnableSoftDeletionGlobalFilter();
        }
    }
}
