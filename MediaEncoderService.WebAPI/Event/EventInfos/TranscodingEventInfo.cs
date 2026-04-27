namespace MediaEncoderService.WebAPI.Event.EventInfos
{
    public record TranscodingEventInfo(MultilingualString Name, Guid AlbumId, Uri AudioUrl, double DurationInSecond, string Subtitle, string SubtitleType, string OutputFormat, string SourceSystem);
}

