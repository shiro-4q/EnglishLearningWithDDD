using FileService.Infrastructure.Persistence;
using ListeningService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ListeningService.Infrastructure.Repositories
{
    public class ListeningRepository(LSDbContext lSDbContext) : IListeningRepository
    {
        private readonly LSDbContext _dbContext = lSDbContext;

        public Task<Category[]> GetCategoriesAsync()
        {
            return _dbContext.Categories.OrderBy(c => c.SequenceNumber).ToArrayAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid categoryId)
        {
            // Find会优先查询内存中的数据，如果没有才会用sql查询，查询主键的情况尽量用find，查询条件非主键时用first和where
            var category = await _dbContext.Categories.FindAsync(categoryId);
            return category;
        }

        public Task<int> GetMaxSeqOfCategoriesAsync()
        {
            return _dbContext.Categories.MaxAsync(c => c.SequenceNumber);
        }

        public async Task<Album?> GetAlbumByIdAsync(Guid albumId)
        {
            var album = await _dbContext.Albums.FindAsync(albumId);
            return album;
        }

        public Task<Album[]> GetAlbumsByCategoryIdAsync(Guid categoryId)
        {
            return _dbContext.Albums.Where(x => x.CategoryId == categoryId).ToArrayAsync();
        }

        public Task<int> GetMaxSeqOfAlbumsAsync(Guid categoryId)
        {
            return _dbContext.Albums.Where(x => x.CategoryId == categoryId).MaxAsync(x => x.SequenceNumber);
        }

        public async Task<Episode?> GetEpisodeByIdAsync(Guid episodeId)
        {
            var episode = await _dbContext.Episodes.FindAsync(episodeId);
            return episode;
        }

        public Task<Episode[]> GetEpisodesByAlbumIdAsync(Guid albumId)
        {
            return _dbContext.Episodes.Where(x => x.AlbumId == albumId).ToArrayAsync();
        }

        public Task<int> GetMaxSeqOfEpisodesAsync(Guid albumId)
        {
            return _dbContext.Episodes.Where(x => x.AlbumId == albumId).MaxAsync(x => x.SequenceNumber);
        }
    }
}
