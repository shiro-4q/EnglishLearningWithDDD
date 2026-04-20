using FileService.Domain.Entities;
using FileService.Domain.Repositories;
using FileService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FileService.Infrastructure.Repositories
{
    public class FSRepository(FSDbContext dbContext) : IFSRepository
    {
        private readonly FSDbContext _dbContext = dbContext;

        public Task<UploadItem?> FindAsync(long fileSize, string fileSHA256Hash)
        {
            return _dbContext.UploadItems.FirstOrDefaultAsync(u => u.FileSizeInBytes == fileSize && u.FileSHA256Hash == fileSHA256Hash);
        }

        public Task AddAsync(UploadItem uploadItem)
        {
            _dbContext.UploadItems.Add(uploadItem);
            return Task.CompletedTask;
        }
    }
}
