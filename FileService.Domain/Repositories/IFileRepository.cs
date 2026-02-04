using FileService.Domain.Entities;

namespace FileService.Domain.Repositories
{
    public interface IFileRepository
    {
        Task<UploadItem> FindAsync(long fileSize, string fileSHA256Hash);

        Task AddAsync(UploadItem uploadItem);
    }
}
