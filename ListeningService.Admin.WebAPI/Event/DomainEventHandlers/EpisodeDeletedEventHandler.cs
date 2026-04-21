using ListeningService.Admin.WebAPI.Event.EventInfos;
using ListeningService.Domain.Events;
using MediatR;
using Q.EventBus;

namespace ListeningService.Admin.WebAPI.Event.DomainEventHandlers
{
    public class EpisodeDeletedEventHandler(IEventBus eventBus) : INotificationHandler<EpisodeDeletedEvent>
    {
        private readonly IEventBus _eventBus = eventBus;

        public Task Handle(EpisodeDeletedEvent notification, CancellationToken cancellationToken)
        {
            // 发布集成事件，实现删除索引，记录日志等功能
            return _eventBus.PublishAsync("episode.deleted", new EpisodeDeletedEventInfo(notification.Id));
        }
    }
}
