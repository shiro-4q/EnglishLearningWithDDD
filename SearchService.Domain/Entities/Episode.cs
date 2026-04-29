namespace SearchService.Domain.Entities
{
    public record Episode(Guid Id, string ChineseName, string EnglishName, string PlainSubtitle, Guid AlbumId);
}
