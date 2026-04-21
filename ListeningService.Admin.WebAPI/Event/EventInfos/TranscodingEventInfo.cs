namespace ListeningService.Admin.WebAPI.Event.EventInfos
{
    public record TranscodingEventInfo(Guid Id, MultilingualString Name, Guid AlbumId, Uri AudioUrl, double DurationInSecond, string Subtitle, string SubtitleType, string OutputFormat, string SourceSystem);
}
