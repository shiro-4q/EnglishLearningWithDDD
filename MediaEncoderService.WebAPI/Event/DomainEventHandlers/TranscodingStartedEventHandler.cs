namespace MediaEncoderService.WebAPI.Event.DomainEventHandlers
{
    public class TranscodingStartedEventHandler(IEventBus eventBus) : INotificationHandler<TranscodingStartedEvent>
    {
        public Task Handle(TranscodingStartedEvent notification, CancellationToken cancellationToken)
        {
            // 将事件发布到事件总线，以便其他组件可以订阅和处理
            return eventBus.PublishAsync("transcoding.started", notification);
        }
    }
}
