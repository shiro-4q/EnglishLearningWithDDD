using MediaEncoderService.Domain.Entities;
using MediaEncoderService.Domain.Enums;

namespace MediaEncoderService.Domain.Repositories
{
    public interface ITranscodingRepository
    {
        Task<TranscodingItem?> GetByIdAsync(Guid id);
        Task<TranscodingItem?> FindCompletedByHashAsync(string fileSHA256Hash, long fileSizeInBytes);
        Task<TranscodingItem[]> FindByStatusAsync(ItemStatus status);
        Task AddAsync(TranscodingItem item);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
