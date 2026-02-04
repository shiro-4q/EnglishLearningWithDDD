using Q.DomainCommons.Models;

namespace FileService.Domain.Entities
{
    public class UploadItem : BaseEntity, IHasCreationTime
    {
        public DateTime CreationTime { get; init; }

        public string FileName { get; init; } = null!;

        public long FileSizeInBytes { get; init; }

        public string FileSHA256Hash { get; init; } = null!;

        public Uri BackupUrl { get; init; } = null!;

        public Uri RemoteUrl { get; init; } = null!;

        private UploadItem()
        {
        }

        public UploadItem(string fileName, long fileSizeInBytes, string fileSHA256Hash, Uri backupUrl, Uri remoteUrl)
        {
            FileName = fileName;
            FileSizeInBytes = fileSizeInBytes;
            FileSHA256Hash = fileSHA256Hash;
            BackupUrl = backupUrl;
            RemoteUrl = remoteUrl;
            CreationTime = DateTime.Now;
        }
    }
}
