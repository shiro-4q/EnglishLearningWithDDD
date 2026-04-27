using MediaEncoderService.Domain.Repositories;
using MediaEncoderService.Domain.Transcoder;

namespace MediaEncoderService.WebAPI.BgServices
{
    public class TranscodeBgService(ITranscodingRepository repository, ITranscoder transcoder) : BackgroundService
    {
        private readonly int restSeconds = 30;// 每次循环休息的秒数
        private readonly ITranscodingRepository _repository = repository;
        private readonly ITranscoder _transcoder = transcoder;
        // 下载原文件，转码，上传转码文件，更新状态
        private async Task DownloadOriginalFileAsync(TranscodingItem item, CancellationToken ct)
        {

        }

        private async Task TranscodeFileAsync(TranscodingItem item, CancellationToken ct)
        {

        }

        private async Task UploadTranscodedFileAsync(TranscodingItem item, CancellationToken ct)
        {

        }

        private async Task ProcessItemAsync(TranscodingItem item, CancellationToken ct)
        {

        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                var readyItems = await _repository.FindByStatusAsync(ItemStatus.Ready);
                foreach (var readyItem in readyItems)
                {
                    try
                    {
                        _transcoder.CanTranscode("");
                        await ProcessItemAsync(readyItem, ct);
                    }
                    catch (Exception ex)
                    {
                        readyItem.Fail(ex);
                    }
                    await _repository.SaveChangesAsync(ct);
                }
                await Task.Delay(restSeconds * 1000, ct);
            }
        }

    }
}
