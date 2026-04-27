using MediaEncoderService.Domain.Repositories;
using MediaEncoderService.Domain.Transcoder;
using Microsoft.Extensions.Options;
using RedLockNet;
using System.Net.Http.Headers;
using System.Security.Cryptography;

namespace MediaEncoderService.WebAPI.BgServices
{
    public class TranscodeBgService : BackgroundService
    {
        private readonly IServiceScope _scope;
        private readonly ITranscodingRepository _repository;
        private readonly TranscoderFactory _transcoderFactory;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDistributedLockFactory _distributedLockFactory;
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
            _scope = scopeFactory.CreateScope();
            _repository = _scope.ServiceProvider.GetRequiredService<ITranscodingRepository>();
            _transcoderFactory = _scope.ServiceProvider.GetRequiredService<TranscoderFactory>();
        }

        private async Task<FileInfo> DownloadOriginalFileAsync(TranscodingItem item, CancellationToken ct)
        {
            var httpClient = _httpClientFactory.CreateClient();
            using var response = await httpClient.GetAsync(item.SourceUrl, HttpCompletionOption.ResponseHeadersRead, ct);
            response.EnsureSuccessStatusCode();

            var fileExtension = Path.GetExtension(item.SourceUrl.LocalPath);
            var filePath = Path.Combine(GetWorkingDirectory(), $"{item.Id:N}-source{fileExtension}");
            await using var sourceStream = await response.Content.ReadAsStreamAsync(ct);
            await using var fileStream = File.Create(filePath);
            await sourceStream.CopyToAsync(fileStream, ct);
            await fileStream.FlushAsync(ct);

            return new FileInfo(filePath);
        }

        private async Task<FileInfo> TranscodeFileAsync(TranscodingItem item, FileInfo sourceFile, TranscoderFactory transcoderFactory, CancellationToken ct)
        {
            var outputFileName = $"{Path.GetFileNameWithoutExtension(item.Name)}-{item.Id:N}.{item.OutputFormat.TrimStart('.')}";
            var outputFile = new FileInfo(Path.Combine(GetWorkingDirectory(), outputFileName));
            await transcoderFactory.TranscodeAsync(sourceFile, outputFile, item.OutputFormat, null, ct);
            return outputFile;
        }

        private async Task<Uri> UploadTranscodedFileAsync(FileInfo outputFile, CancellationToken ct)
        {
            if (_options.FileServiceUploadUrl == null)
            {
                throw new InvalidOperationException("FileService upload url is not configured.");
            }

            var httpClient = _httpClientFactory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, _options.FileServiceUploadUrl);
            if (!string.IsNullOrWhiteSpace(_options.FileServiceAccessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.FileServiceAccessToken);
            }

            await using var fileStream = outputFile.OpenRead();
            using var content = new MultipartFormDataContent();
            content.Add(new StreamContent(fileStream), "File", outputFile.Name);
            request.Content = content;

            using var response = await httpClient.SendAsync(request, ct);
            response.EnsureSuccessStatusCode();
            var uploadUrl = await response.Content.ReadAsStringAsync(ct);
            return new Uri(uploadUrl.Trim('"'));
        }

        private async Task ProcessItemAsync(TranscodingItem item, ITranscodingRepository repository, TranscoderFactory transcoderFactory, CancellationToken ct)
        {
            if (!transcoderFactory.CanTranscode(item.OutputFormat))
            {
                throw new NotSupportedException($"No transcoder found for format: {item.OutputFormat}");
            }

            await using var redLock = await _distributedLockFactory.CreateLockAsync(
                $"MediaEncoderService:TranscodingItem:{item.Id}",
                _options.LockExpiry,
                _options.LockWait,
                _options.LockRetry,
                ct);
            if (!redLock.IsAcquired)
            {
                return;
            }

            FileInfo? sourceFile = null;
            FileInfo? outputFile = null;
            try
            {
                item.Start();
                await repository.SaveChangesAsync(ct);

                sourceFile = await DownloadOriginalFileAsync(item, ct);
                var (sourceFileSize, sourceFileHash) = await GetFileMetadataAsync(sourceFile, ct);
                item.ChangeFileMetadata(sourceFileSize, sourceFileHash);
                await repository.SaveChangesAsync(ct);

                outputFile = await TranscodeFileAsync(item, sourceFile, transcoderFactory, ct);
                var outputUrl = await UploadTranscodedFileAsync(outputFile, ct);
                item.Complete(outputUrl);
            }
            finally
            {
                DeleteFileIfExists(sourceFile);
                DeleteFileIfExists(outputFile);
            }
        }

        private static async Task<(long FileSizeInBytes, string FileSHA256Hash)> GetFileMetadataAsync(FileInfo file, CancellationToken ct)
        {
            await using var stream = file.OpenRead();
            var hashBytes = await SHA256.HashDataAsync(stream, ct);
            var hash = Convert.ToHexString(hashBytes).ToLowerInvariant();
            return (file.Length, hash);
        }

        private static void DeleteFileIfExists(FileInfo? file)
        {
            file?.Refresh();
            if (file?.Exists == true)
            {
                file.Delete();
            }
        }

        private string GetWorkingDirectory()
        {
            var workingDirectory = string.IsNullOrWhiteSpace(_options.WorkingDirectory)
                ? Path.Combine(Path.GetTempPath(), "MediaEncoderService")
                : _options.WorkingDirectory;
            Directory.CreateDirectory(workingDirectory);
            return workingDirectory;
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
                        await ProcessItemAsync(readyItem, _repository, _transcoderFactory, ct);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to transcode item {ItemId}.", readyItem.Id);
                        readyItem.Fail(ex);
                    }
                    await _repository.SaveChangesAsync(ct);
                }
                await Task.Delay(_options.RestInterval, ct);
            }
        }
    }

    public class TranscodeBgServiceOptions
    {
        public int RestSeconds { get; set; } = 30;
        public string? WorkingDirectory { get; set; }
        public Uri? FileServiceUploadUrl { get; set; }
        public string? FileServiceAccessToken { get; set; }
        public TimeSpan LockExpiry { get; set; } = TimeSpan.FromMinutes(30);
        public TimeSpan LockWait { get; set; } = TimeSpan.Zero;
        public TimeSpan LockRetry { get; set; } = TimeSpan.FromSeconds(1);
        public TimeSpan RestInterval => TimeSpan.FromSeconds(RestSeconds);
    }

}
