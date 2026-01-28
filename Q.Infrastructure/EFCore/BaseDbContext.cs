using MediatR;
using Microsoft.EntityFrameworkCore;
using Q.DomainCommons.Models;
using System.Reflection;

namespace Q.Infrastructure.EFCore
{
    public class BaseDbContext(IMediator mediator) : DbContext
    {
        private readonly IMediator _mediator = mediator;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly());
        }

        public override int SaveChanges()
        {
            throw new ApplicationException("Do not call SaveChanges, please call SaveChangesAsync instead");
        }

        //另一个重载的SaveChangesAsync(CancellationToken cancellationToken)也调用这个SaveChangesAsync方法。所以在这里统一控制
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var softDeletedEntries = ChangeTracker.Entries<ISoftDelete>()
                .Where(e => e.State == EntityState.Modified && e.Entity.IsDeleted).ToList();//在提交到数据库之前，记录那些被“软删除”实体对象。一定要ToList()，否则会延迟到ForEach的时候才执行

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            //把被软删除的对象从Cache删除，否则FindAsync()还能根据Id获取到这条数据
            //因为FindAsync如果能从本地Cache找到，就不会去数据库上查询，而从本地Cache找的过程中不会管QueryFilter
            //就会造成已经软删除的数据仍然能够通过FindAsync查到的问题，因此这里把对应跟踪对象的state改为Detached，就会从缓存中删除了
            softDeletedEntries.ForEach(e => e.State = EntityState.Detached);

            //分发领域事件
            if (_mediator != null)
                await _mediator.DispatchDomainEventsAsync(this);

            return result;
        }
    }
}
