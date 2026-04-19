using ListeningService.Domain.Entities;
using MediatR;

namespace ListeningService.Domain.Events
{
    public record CategoryUpdatedEvent(Category Category) : INotification;
}
