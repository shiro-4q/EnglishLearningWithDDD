namespace Q.EventBus
{
    public interface IEventBus
    {
        void Publish(string eventName, object? eventData);
        Task PublishAsync(string eventName, object? eventData);
    }
}
