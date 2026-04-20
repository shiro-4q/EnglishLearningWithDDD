namespace ListeningService.Admin.WebAPI.DTOs.Requests
{
    public record EpisodeUpdateRequest(Guid EpisodeId, MultilingualString Name, string Subtitle, string SubtitleType);
}
