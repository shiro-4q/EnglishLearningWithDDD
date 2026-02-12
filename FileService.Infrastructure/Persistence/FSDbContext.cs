using FileService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Q.Infrastructure.EFCore;
using System.Reflection;

namespace FileService.Infrastructure.Persistence
{
    public class FSDbContext(DbContextOptions<FSDbContext> options, IMediator? mediator) : BaseDbContext(options, mediator)
    {
        public DbSet<UploadItem> UploadItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
