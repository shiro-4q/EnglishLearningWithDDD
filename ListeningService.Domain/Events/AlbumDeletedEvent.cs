using MediatR;

namespace ListeningService.Domain.Events
{
    public record AlbumDeletedEvent(Guid Id) : INotification;
}
