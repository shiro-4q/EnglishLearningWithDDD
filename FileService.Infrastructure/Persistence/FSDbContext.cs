using FileService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Q.Infrastructure.EFCore;

namespace FileService.Infrastructure.Persistence
{
    public class FSDbContext(DbContextOptions<FSDbContext> options, IMediator? mediator) : BaseDbContext(options, mediator)
    {
        public DbSet<Person> Persons
        {
            get; set;
        }
    }
}
