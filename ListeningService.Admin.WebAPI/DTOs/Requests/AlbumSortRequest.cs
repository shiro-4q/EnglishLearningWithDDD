namespace ListeningService.Admin.WebAPI.DTOs.Requests
{
    public record AlbumSortRequest(Guid CategoryId, Guid[] AlbumIds);
}
