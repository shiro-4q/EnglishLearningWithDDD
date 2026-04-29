using SearchService.Domain.Entities;

namespace SearchService.Domain.Responses
{
    public record SearchEpisodeResponse(IEnumerable<Episode> Episodes, long TotalCount);
}
