namespace ListeningService.Admin.WebAPI.DTOs.Requests
{
    public record EpisodeSortRequest(Guid AlbumId, Guid[] EpisodeIds);
}
