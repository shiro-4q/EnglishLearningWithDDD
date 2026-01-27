using MediatR;

namespace Q.DomainCommons
{
    public interface IDomainEvents
    {
        void ClearDomainEvents();
        void AddDomainEvent(INotification eventItem);
        void AddDomainEventsIfAbsent(INotification eventItem);
        IEnumerable<INotification> GetDomainEvents();
    }
}
