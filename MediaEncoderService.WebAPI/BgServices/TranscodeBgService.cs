using FileService.WebAPI.Helpers;
using MediaEncoderService.Domain.Repositories;
using MediaEncoderService.Domain.Transcoder;
using Microsoft.Extensions.Options;
using RedLockNet;

namespace MediaEncoderService.WebAPI.BgServices
{
    public class TranscodeBgService : BackgroundService
    {
        private readonly IServiceScope _scope;
        private readonly ITranscodingRepository _repository;
        private readonly TranscoderFactory _transcoderFactory;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDistributedLockFactory _distributedLockFactory;
        private readonly FileServiceUploadHelper _fileServiceUploadHelper;
        private readonly TranscodeBgServiceOptions _options;
        private readonly ILogger<TranscodeBgService> _logger;

        public TranscodeBgService(
            ILogger<TranscodeBgService> logger,
            IOptions<TranscodeBgServiceOptions> options,
            IDistributedLockFactory distributedLockFactory,
            IHttpClientFactory httpClientFactory,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _options = options.Value;
            _distributedLockFactory = distributedLockFactory;
            _httpClientFactory = httpClientFactory;
            // BgService是使用Singleton生命周期注册的，如果直接注入Scoped和Transient生命周期的服务，会导致生命周期不匹配
            // 所以需要创建一个scope来获取Scoped和Transient生命周期服务
            _scope = scopeFactory.CreateScope();
            _repository = _scope.ServiceProvider.GetRequiredService<ITranscodingRepository>();
            _transcoderFactory = _scope.ServiceProvider.GetRequiredService<TranscoderFactory>();
            _fileServiceUploadHelper = _scope.ServiceProvider.GetRequiredService<FileServiceUploadHelper>();
        }

        /// <summary>
        /// 下载源文件到本地工作目录
        /// </summary>
        private async Task<FileInfo> DownloadOriginalFileAsync(TranscodingItem item, CancellationToken ct)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var fileExtension = Path.GetExtension(item.SourceUrl.LocalPath);
            var filePath = Path.Combine(GetWorkingDirectory(), $"{item.Id:N}-source{fileExtension}");
            await httpClient.DownloadFileAsync(item.SourceUrl, filePath, ct);
            return new FileInfo(filePath);
        }

        /// <summary>
        /// 把本地文件转码成目标格式
        /// </summary>
        private async Task<FileInfo> TranscodeFileAsync(TranscodingItem item, FileInfo sourceFile, TranscoderFactory transcoderFactory, CancellationToken ct)
        {
            var outputFileName = $"{Path.GetFileNameWithoutExtension(item.Name)}-{item.Id:N}.{item.OutputFormat.TrimStart('.')}";
            var outputPath = Path.Combine(GetWorkingDirectory(), outputFileName);
            var outputFile = new FileInfo(outputPath);
            await transcoderFactory.TranscodeAsync(sourceFile, outputFile, item.OutputFormat, null, ct);
            return outputFile;
        }

        /// <summary>
        /// 上传转码后的文件到FileService
        /// </summary>
        private Task<Uri> UploadTranscodedFileAsync(FileInfo outputFile, CancellationToken ct)
        {
            if (_options.FileServiceUploadUrl == null)
            {
                throw new InvalidOperationException("FileService upload url is not configured.");
            }

            return _fileServiceUploadHelper.UploadAsync(_options.FileServiceUploadUrl, outputFile, ct);
        }

        /// <summary>
        /// 处理单个转码任务
        /// </summary>
        private async Task ProcessItemAsync(TranscodingItem item, ITranscodingRepository repository, TranscoderFactory transcoderFactory, CancellationToken ct)
        {
            if (!transcoderFactory.CanTranscode(item.OutputFormat))
            {
                throw new NotSupportedException($"No transcoder found for format: {item.OutputFormat}");
            }
            // Redis分布式锁来避免两个转码服务器处理同一个转码任务的问题
            // 用RedLock分布式锁，锁定对TranscodingItem的访问
            await using var redLock = await _distributedLockFactory.CreateLockAsync(
                $"MediaEncoderService:TranscodingItem:{item.Id}",
                _options.LockExpiry,
                _options.LockWait,
                _options.LockRetry,
                ct);
            if (!redLock.IsAcquired)
            {
                //获得锁失败，锁已经被别人抢走了，说明这个任务被别的实例处理了（有可能有服务器集群来分担转码压力）
                return;//再去抢下一个
            }

            FileInfo? sourceFile = null;
            FileInfo? outputFile = null;
            try
            {
                item.Start();
                await repository.SaveChangesAsync(ct);// 立即保存一次，把事件发布出去，这样外部系统就能知道转码任务开始了
                _logger.LogInformation("Started processing item {ItemId}.", item.Id);

                sourceFile = await DownloadOriginalFileAsync(item, ct);
                var sourceFileSize = sourceFile.Length;
                await using (var sourceStream = sourceFile.OpenRead())
                {
                    var sourceFileHash = HashHelper.ComputeSha256Hash(sourceStream);
                    item.ChangeFileMetadata(sourceFileSize, sourceFileHash);

                    var completedItem = await repository.FindCompletedByHashAsync(sourceFileHash, sourceFileSize);
                    if (completedItem?.OutputUrl != null)
                    {
                        item.Complete(completedItem.OutputUrl);
                        _logger.LogInformation("Item {ItemId} completed.", item.Id);
                        return;
                    }
                }

                outputFile = await TranscodeFileAsync(item, sourceFile, transcoderFactory, ct);
                var outputUrl = await UploadTranscodedFileAsync(outputFile, ct);
                item.Complete(outputUrl);
                _logger.LogInformation("Item {ItemId} completed.", item.Id);
            }
            finally
            {
                DeleteFileIfExists(sourceFile);
                DeleteFileIfExists(outputFile);
            }
        }

        /// <summary>
        /// 删除临时文件
        /// </summary>
        private static void DeleteFileIfExists(FileInfo? file)
        {
            file?.Refresh();
            if (file?.Exists == true)
            {
                file.Delete();
            }
        }

        /// <summary>
        /// 获取本地工作目录
        /// </summary>
        private string GetWorkingDirectory()
        {
            var workingDirectory = string.IsNullOrWhiteSpace(_options.WorkingDirectory)
                ? Path.Combine(Path.GetTempPath(), "MediaEncoderService")
                : _options.WorkingDirectory;
            Directory.CreateDirectory(workingDirectory);
            return workingDirectory;
        }

        /// <summary>
        /// 定时扫描并执行待处理的转码任务
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                var readyItems = await _repository.FindByStatusAsync(ItemStatus.Ready);
                foreach (var readyItem in readyItems)
                {
                    try
                    {
                        await ProcessItemAsync(readyItem, _repository, _transcoderFactory, ct);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to transcode item {ItemId}.", readyItem.Id);
                        readyItem.Fail(ex);
                    }
                    await _repository.SaveChangesAsync(ct);
                }
                await Task.Delay(_options.RestInterval, ct);// 设置置扫描间隔，避免频繁查询数据库造成数据库和cpu压力
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _scope.Dispose();
        }

    }
}
