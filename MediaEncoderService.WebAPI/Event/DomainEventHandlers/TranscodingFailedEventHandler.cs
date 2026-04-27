namespace MediaEncoderService.WebAPI.Event.DomainEventHandlers
{
    public class TranscodingFailedEventHandler(IEventBus eventBus) : INotificationHandler<TranscodingFailedEvent>
    {
        public Task Handle(TranscodingFailedEvent notification, CancellationToken cancellationToken)
        {
            return eventBus.PublishAsync("transcoding.failed", notification);
        }
    }
}
