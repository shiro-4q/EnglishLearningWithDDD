using IdentityService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Q.Infrastructure.EFCore;
using System.Reflection;

namespace IdentityService.Infrastructure.Persistence
{
    public class IdDbContext(DbContextOptions options, IMediator? mediator) : BaseDbContext(options, mediator)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
