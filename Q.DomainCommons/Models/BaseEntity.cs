using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace Q.DomainCommons.Models
{
    public class BaseEntity : IDomainEvents
    {
        [NotMapped]
        private readonly List<INotification> _domainEvents = [];
        public Guid Id { get; protected set; } = Guid.NewGuid();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents.Add(eventItem);
        }

        public void AddDomainEventsIfAbsent(INotification eventItem)
        {
            if (!_domainEvents.Contains(eventItem))
                _domainEvents.Add(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public IEnumerable<INotification> GetDomainEvents()
        {
            return _domainEvents;
        }
    }
}
