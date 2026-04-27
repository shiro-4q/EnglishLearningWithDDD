using DotNetCore.CAP;
using MediaEncoderService.Domain.Repositories;
using MediaEncoderService.WebAPI.Event.EventInfos;

namespace MediaEncoderService.WebAPI.Event.IntegrationEventHandlers
{
    public class TranscodingSubscriber : ICapSubscribe
    {
        [CapSubscribe("transcoding.created")]
        public async Task OnTranscodingCreatedAsync(TranscodingEventInfo eventInfo, ITranscodingRepository repository)
        {
            // 将事件信息保存到数据库，等待后台服务处理
            //string fileName = eventInfo.AudioUrl.Segments.Last(); // 按照/分段，再获取最后一段作为文件名，语义没有Path.GetFileName清晰
            string fileName = Path.GetFileName(eventInfo.AudioUrl.LocalPath);
            TranscodingItem item = new(fileName, eventInfo.SourceSystem, eventInfo.AudioUrl, eventInfo.OutputFormat);
            await repository.AddAsync(item);
            await repository.SaveChangesAsync();
        }
    }
}
