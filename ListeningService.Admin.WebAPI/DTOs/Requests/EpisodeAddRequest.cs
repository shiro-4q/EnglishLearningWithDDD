namespace ListeningService.Admin.WebAPI.DTOs.Requests
{
    public record EpisodeAddRequest(Guid AlbumId, MultilingualString Name, Uri AudioUrl, double DurationInSecond, string Subtitle, string SubtitleType);
}
