using ListeningService.Domain.Events;
using ListeningService.Domain.ValueObjects;
using Q.DomainCommons.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListeningService.Domain.Entities
{
    /* 项目需求：非m4a文件上传后先转码再发布。传统做法是添加一个状态 标识是否转码完成，但是这么做当没转码成功的时候数据就是非法状态，需要在很多逻辑做状态的判断
    DDD的思想，将非法状态的Episode存到单独的一张表或者缓存中 例如PendingEpisode，当转码成功了再添加到Episode 表中，不会污染实体，所有Episode都是合法的 */
    public class Episode : AggregateRootEntity
    {
        public MultilingualString Name { get; private set; }
        public int SequenceNumber { get; private set; }
        public Guid AlbumId { get; private set; }
        public string Subtitle { get; private set; }
        public string SubtitleType { get; private set; }
        public double DurationInSecond { get; private set; }
        public Uri AudioUrl { get; private set; }
        public bool IsVisible { get; private set; }
        [NotMapped]
        public IEnumerable<Sentence> Sentences { get; private set; }

        private Episode() { }

        public void ChangeName(MultilingualString name)
        {
            Name = name;
            this.AddDomainEventsIfAbsent(new EpisodeUpdatedEvent(this));
        }
        public void ChangeSequenceNumber(int sequenceNumber)
        {
            SequenceNumber = sequenceNumber;
            this.AddDomainEventsIfAbsent(new EpisodeUpdatedEvent(this));
        }
        public void ChangeSubtitle(string subtitle, string subtitleType)
        {
            Subtitle = subtitle;
            SubtitleType = subtitleType;
            this.AddDomainEventsIfAbsent(new EpisodeUpdatedEvent(this));
        }
        public void Show()
        {
            IsVisible = true;
            this.AddDomainEventsIfAbsent(new EpisodeUpdatedEvent(this));
        }
        public void Hide()
        {
            IsVisible = false;
            this.AddDomainEventsIfAbsent(new EpisodeUpdatedEvent(this));
        }
        public void SetSentences(IEnumerable<Sentence> sentences)
        {
            Sentences = sentences;
        }

        public override void SoftDelete()
        {
            base.SoftDelete();
            this.AddDomainEventsIfAbsent(new EpisodeDeletedEvent(this.Id));
        }

        public class Builder
        {
            public Guid id;
            public MultilingualString name;
            public int sequenceNumber;
            public Guid albumId;
            public string subtitle;
            public string subtitleType;
            public double durationInSecond;
            public Uri audioUrl;
            public bool isVisible;

            public Builder Id(Guid value)
            {
                id = value;
                return this;
            }
            public Builder Name(MultilingualString value)
            {
                name = value;
                return this;
            }
            public Builder SequenceNumber(int value)
            {
                sequenceNumber = value;
                return this;
            }
            public Builder AlbumId(Guid value)
            {
                albumId = value;
                return this;
            }
            public Builder Subtitle(string value)
            {
                subtitle = value;
                return this;
            }
            public Builder SubtitleType(string value)
            {
                subtitleType = value;
                return this;
            }
            public Builder DurationInSecond(double value)
            {
                durationInSecond = value;
                return this;
            }
            public Builder AudioUrl(Uri value)
            {
                audioUrl = value;
                return this;
            }
            public Builder IsVisible(bool value)
            {
                isVisible = value;
                return this;
            }

            public Episode Build()
            {
                if (id == Guid.Empty)
                    throw new ArgumentOutOfRangeException(nameof(id));
                if (name == null)
                    throw new ArgumentNullException(nameof(name));
                if (albumId == Guid.Empty)
                    throw new ArgumentOutOfRangeException(nameof(albumId));
                if (audioUrl == null)
                    throw new ArgumentNullException(nameof(audioUrl));
                if (durationInSecond <= 0)
                    throw new ArgumentOutOfRangeException(nameof(durationInSecond));
                if (subtitle == null)
                    throw new ArgumentNullException(nameof(subtitle));
                if (subtitleType == null)
                    throw new ArgumentNullException(nameof(subtitleType));
                Episode episode = new()
                {
                    Id = id,
                    Name = name,
                    SequenceNumber = sequenceNumber,
                    AlbumId = albumId,
                    Subtitle = subtitle,
                    SubtitleType = subtitleType,
                    DurationInSecond = durationInSecond,
                    AudioUrl = audioUrl,
                    IsVisible = isVisible,
                };
                episode.AddDomainEventsIfAbsent(new EpisodeCreatedEvent(episode));
                return episode;
            }
        }
    }
}
