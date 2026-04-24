using MediaEncoderService.Domain.Enums;
using Q.DomainCommons.Models;

namespace MediaEncoderService.Domain.Entities
{
    public class TranscodingItem : BaseEntity, IAggregateRoot, IHasCreationTime
    {
        public DateTime CreationTime { get; init; }
        public string Name { get; init; } = null!;
        public string SourceSystem { get; init; } = null!;
        public long? FileSizeInBytes { get; private set; }
        public string? FileSHA256Hash { get; private set; }
        public Uri SourceUrl { get; init; } = null!;
        public Uri? OutputUrl { get; private set; }
        public string OutputFormat { get; init; } = null!;
        public ItemStatus Status { get; private set; }
        public string? Log { get; private set; }

        private TranscodingItem() { }

        public TranscodingItem(string name, string sourceSystem, Uri sourceUrl, string outputFormat)
        {
            Name = name;
            SourceSystem = sourceSystem;
            SourceUrl = sourceUrl;
            OutputFormat = outputFormat;
            CreationTime = DateTime.Now;
            Status = ItemStatus.Ready;
        }

        public void ChangeFileMetadata(long fileSizeInBytes, string fileSHA256Hash)
        {
            FileSizeInBytes = fileSizeInBytes;
            FileSHA256Hash = fileSHA256Hash;
        }

        public void Complited(Uri outputUrl)
        {
            OutputUrl = outputUrl;
            Status = ItemStatus.Completed;
            Log = "转码完成";
            //this.AddDomainEventsIfAbsent();
        }
    }
}
