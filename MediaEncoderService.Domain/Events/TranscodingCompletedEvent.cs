using MediaEncoderService.Domain.Entities;
using MediatR;

namespace MediaEncoderService.Domain.Events
{
    public record TranscodingCompletedEvent(TranscodingItem TranscodingItem) : INotification;
}
