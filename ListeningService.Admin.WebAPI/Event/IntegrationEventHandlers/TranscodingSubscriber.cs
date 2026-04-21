using DotNetCore.CAP;
using ListeningService.Admin.WebAPI.Event.EventInfos;
using ListeningService.Domain.Repositories;
using ListeningService.Domain.Services;

namespace ListeningService.Admin.WebAPI.Event.IntegrationEventHandlers
{
    public class TranscodingSubscriber : ICapSubscribe
    {
        [CapSubscribe("transcoding.completed")]
        public async Task OnTranscodingCompletedAsync(TranscodingEventInfo eventInfo, IListeningRepository repository, ListeningDomainService domainService)
        {
            await domainService.AddEpisodeAsync(eventInfo.AlbumId, eventInfo.Name, eventInfo.AudioUrl, eventInfo.DurationInSecond, eventInfo.Subtitle, eventInfo.SubtitleType);
            await repository.SaveChangesAsync();
        }
    }
}
