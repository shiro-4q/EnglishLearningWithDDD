using MediaEncoderService.Domain.Enums;
using MediaEncoderService.Domain.Repositories;
using MediaEncoderService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MediaEncoderService.Infrastructure.Repositories
{
    public class TranscodingRepository(TranscodingDbContext dbContext) : ITranscodingRepository
    {
        private readonly TranscodingDbContext _dbContext = dbContext;

        public Task<TranscodingItem[]> FindByStatusAsync(ItemStatus status)
        {
            return _dbContext.TranscodingItems.Where(t => t.Status == status).ToArrayAsync();
        }

        public Task<TranscodingItem?> FindOneByHashAsync(string fileSHA256Hash, long fileSizeInBytes)
        {
            return _dbContext.TranscodingItems.FirstOrDefaultAsync(t => t.FileSHA256Hash == fileSHA256Hash && t.FileSizeInBytes == fileSizeInBytes);
        }

        public async Task<TranscodingItem?> GetByIdAsync(Guid id)
        {
            var item = await _dbContext.TranscodingItems.FindAsync(id);
            return item;
        }

        public async Task AddAsync(TranscodingItem item)
        {
            await _dbContext.TranscodingItems.AddAsync(item);
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
        {
            return _dbContext.SaveChangesAsync(ct);
        }
    }
}
