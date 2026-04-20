namespace ListeningService.Admin.WebAPI.DTOs.Requests
{
    public record AlbumAddRequest(Guid CategoryId, MultilingualString Name);
}