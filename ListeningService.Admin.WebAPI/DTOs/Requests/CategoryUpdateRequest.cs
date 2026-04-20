namespace ListeningService.Admin.WebAPI.DTOs.Requests
{
    public record CategoryUpdateRequest(Guid CategoryId, MultilingualString Name, Uri CoverUrl);
}
