using MediatR;
using Microsoft.EntityFrameworkCore;
using Q.Infrastructure.EFCore;

namespace MediaEncoderService.Infrastructure.Persistence
{
    public class TranscodingDbContext(DbContextOptions options, IMediator? mediator) : BaseDbContext(options, mediator)
    {
        public DbSet<TranscodingItem> TranscodingItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
