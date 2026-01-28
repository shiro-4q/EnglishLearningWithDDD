using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Q.DomainCommons.Models;

namespace Q.Infrastructure.EFCore
{
    public static class MediatorRExtensions
    {
        public static IServiceCollection AddMediatorR(this IServiceCollection services)
        {
            return services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });
        }

        /// <summary>
        /// 分发领域事件(写到base.SaveChangesAsync之后，保证发布的领域事件都是已经成功保存到数据库的数据 )
        /// </summary>
        /// <param name="mediator">mediator实体</param>
        /// <param name="ctx">数据库上下文</param>
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, DbContext ctx)
        {
            var domainEventEntries = ctx.ChangeTracker.Entries<IDomainEvents>()
                .Where(e => e.Entity.GetDomainEvents().Any());
            var domainEvents = domainEventEntries.SelectMany(d => d.Entity.GetDomainEvents()).ToList();
            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }
            domainEventEntries.ToList().ForEach(e => e.Entity.ClearDomainEvents());
        }
    }
}
