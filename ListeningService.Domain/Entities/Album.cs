using ListeningService.Domain.Events;
using Q.DomainCommons.Models;

namespace ListeningService.Domain.Entities
{
    public class Album : AggregateRootEntity
    {
        public MultilingualString Name { get; private set; }
        public int SequenceNumber { get; private set; }

        // 不同的聚合，不要直接引用实体，而是只引用主键，方便拆分
        public Guid CategoryId { get; private set; }

        private Album() { }

        public Album(MultilingualString name, int sequenceNumber, Guid categoryId)
        {
            Name = name;
            SequenceNumber = sequenceNumber;
            CategoryId = categoryId;
            this.AddDomainEventsIfAbsent(new AlbumCreatedEvent(this));
        }

        public void ChangeName(MultilingualString name)
        {
            Name = name;
            this.AddDomainEventsIfAbsent(new AlbumUpdatedEvent(this));
        }

        public void ChangeSequenceNumber(int sequenceNumber)
        {
            SequenceNumber = sequenceNumber;
            this.AddDomainEventsIfAbsent(new AlbumUpdatedEvent(this));
        }
    }
}
