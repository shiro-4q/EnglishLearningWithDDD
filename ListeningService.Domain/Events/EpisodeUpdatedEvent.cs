using ListeningService.Domain.Entities;
using MediatR;

namespace ListeningService.Domain.Events
{
    public record EpisodeUpdatedEvent(Episode Episode) : INotification;
}
