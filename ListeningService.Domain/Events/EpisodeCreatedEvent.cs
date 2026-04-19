using ListeningService.Domain.Entities;
using MediatR;

namespace ListeningService.Domain.Events
{
    public record EpisodeCreatedEvent(Episode Episode) : INotification;
}
