namespace MediaEncoderService.WebAPI.Event.DomainEventHandlers
{
    public class TranscodingCompletedEventHandler(IEventBus eventBus) : INotificationHandler<TranscodingCompletedEvent>
    {
        public Task Handle(TranscodingCompletedEvent notification, CancellationToken cancellationToken)
        {
            return eventBus.PublishAsync("transcoding.completed", notification);
        }
    }
}
