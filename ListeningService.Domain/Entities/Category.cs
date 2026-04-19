using ListeningService.Domain.Events;
using Q.DomainCommons.Models;

namespace ListeningService.Domain.Entities
{
    public class Category : AggregateRootEntity
    {
        public MultilingualString Name { get; private set; }
        public int SequenceNumber { get; private set; }
        public Uri CoverUrl { get; private set; }

        private Category() { }

        public Category(MultilingualString name, int sequenceNumber, Uri coverUrl)
        {
            Name = name;
            SequenceNumber = sequenceNumber;
            CoverUrl = coverUrl;
            this.AddDomainEventsIfAbsent(new CategoryCreatedEvent(this));
        }

        public void ChangeName(MultilingualString name)
        {
            Name = name;
            this.AddDomainEventsIfAbsent(new CategoryUpdatedEvent(this));
        }

        public void ChangeSequenceNumber(int sequenceNumber)
        {
            SequenceNumber = sequenceNumber;
            this.AddDomainEventsIfAbsent(new CategoryUpdatedEvent(this));
        }

        public void ChangeCoverUrl(Uri coverUrl)
        {
            CoverUrl = coverUrl;
            this.AddDomainEventsIfAbsent(new CategoryUpdatedEvent(this));
        }
    }
}
