using MediatR;

namespace ListeningService.Domain.Events
{
    public record EpisodeDeletedEvent(Guid Id) : INotification;
}
