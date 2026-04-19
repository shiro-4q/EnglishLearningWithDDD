using MediatR;

namespace ListeningService.Domain.Events
{
    public record CategoryDeletedEvent(Guid Id) : INotification;
}
