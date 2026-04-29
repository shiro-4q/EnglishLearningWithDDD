using SearchService.Domain.Entities;
using SearchService.Domain.Responses;

namespace SearchService.Domain.Repositories
{
    public interface ISearchRepository
    {
        Task UpsertAsync(Episode episode);
        Task DeleteAsync(Guid id);
        Task<SearchEpisodeResponse> SearchAsync(string keyword, int pageIndex, int pageSize);
    }
}
