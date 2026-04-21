using ListeningService.Admin.WebAPI.Event.EventInfos;
using ListeningService.Domain.Events;
using MediatR;
using Q.EventBus;

namespace ListeningService.Admin.WebAPI.Event.DomainEventHandlers
{
    public class EpisodeUpdatedEventHandler(IEventBus eventBus) : INotificationHandler<EpisodeUpdatedEvent>
    {
        private readonly IEventBus _eventBus = eventBus;

        public Task Handle(EpisodeUpdatedEvent notification, CancellationToken cancellationToken)
        {
            // 发布集成事件，实现更新索引，记录日志等功能
            return _eventBus.PublishAsync("episode.updated", new EpisodeUpdatedEventInfo(notification.Episode));
        }
    }
}
