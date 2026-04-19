using ListeningService.Domain.Entities;
using MediatR;

namespace ListeningService.Domain.Events
{
    public record CategoryCreatedEvent(Category Category) : INotification;
}
