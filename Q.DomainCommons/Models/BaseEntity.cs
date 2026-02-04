using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace Q.DomainCommons.Models
{
    public class BaseEntity : IDomainEvents
    {
        [NotMapped]
        private readonly List<INotification> _domainEvents = [];

        // 使用Version 7 UUID，解决老版本UUId的排序问题，需要.net 9+
        public Guid Id { get; protected set; } = Guid.CreateVersion7();

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
