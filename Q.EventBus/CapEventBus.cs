using DotNetCore.CAP;

namespace Q.EventBus
{
    public class CapEventBus(ICapPublisher capPublisher) : IEventBus
    {
        private readonly ICapPublisher _capPublisher = capPublisher;

        public void Publish(string eventName, object? eventData)
        {
            _capPublisher.Publish(eventName, eventData);
        }

        public Task PublishAsync(string eventName, object? eventData)
        {
            return _capPublisher.PublishAsync(eventName, eventData);
        }
    }
}
