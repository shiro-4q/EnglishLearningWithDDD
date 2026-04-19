using ListeningService.Domain.Entities;
using MediatR;

namespace ListeningService.Domain.Events
{
    public record AlbumUpdatedEvent(Album Album) : INotification;
}
