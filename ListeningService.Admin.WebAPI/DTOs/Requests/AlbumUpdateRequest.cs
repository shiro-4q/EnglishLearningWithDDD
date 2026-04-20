namespace ListeningService.Admin.WebAPI.DTOs.Requests
{
    public record AlbumUpdateRequest(Guid AlbumId, MultilingualString Name);
}