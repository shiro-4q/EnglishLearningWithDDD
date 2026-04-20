using ListeningService.Domain.Entities;

namespace ListeningService.Domain.Repositories
{
    public interface IListeningRepository
    {
        Task<Category?> GetCategoryByIdAsync(Guid categoryId);
        Task<Category[]> GetCategoriesAsync();
        Task<int> GetMaxSeqOfCategoriesAsync();
        Task AddCategoryAsync(Category category);
        Task<Album?> GetAlbumByIdAsync(Guid albumId);
        Task<Album[]> GetAlbumsByCategoryIdAsync(Guid categoryId);
        Task<int> GetMaxSeqOfAlbumsAsync(Guid categoryId);
        Task AddAlbumAsync(Album album);
        Task<Episode?> GetEpisodeByIdAsync(Guid albumId);
        Task<Episode[]> GetEpisodesByAlbumIdAsync(Guid albumId);
        Task<int> GetMaxSeqOfEpisodesAsync(Guid albumId);
        Task AddEpisodeAsync(Episode episode);
    }
}
