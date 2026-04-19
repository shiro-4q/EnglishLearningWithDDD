using ListeningService.Domain.Entities;
using MediatR;

namespace ListeningService.Domain.Events
{
    public record AlbumCreatedEvent(Album Album) : INotification;
}
