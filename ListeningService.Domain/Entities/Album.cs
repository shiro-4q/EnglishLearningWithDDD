using ListeningService.Domain.Events;
using Q.DomainCommons.Models;

namespace ListeningService.Domain.Entities
{
    public class Album : AggregateRootEntity
    {
        public MultilingualString Name { get; private set; }
        public int SequenceNumber { get; private set; }
        public bool IsVisible { get; private set; }

        // 不同的聚合，不要直接引用实体，而是只引用主键，方便拆分
        public Guid CategoryId { get; private set; }

        private Album() { }

        public Album(MultilingualString name, int sequenceNumber, Guid categoryId)
        {
            Name = name;
            SequenceNumber = sequenceNumber;
            CategoryId = categoryId;
            IsVisible = false;// 默认新建的专辑不可见，需要后台管理员设置成可见后才会展示在前台
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

        public void Show()
        {
            IsVisible = true;
            this.AddDomainEventsIfAbsent(new AlbumUpdatedEvent(this));
        }

        public void Hide()
        {
            IsVisible = false;
            this.AddDomainEventsIfAbsent(new AlbumUpdatedEvent(this));
        }
    }
}
