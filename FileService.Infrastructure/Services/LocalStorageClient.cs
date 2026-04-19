using FileService.Domain.Enums;
using FileService.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace FileService.Infrastructure.Services
{
    /// <summary>
    /// 存储文件到本地目录的存储客户端，主要用于测试和开发环境，生产环境不应该使用它
    /// </summary>
    public class LocalStorageClient(IOptionsSnapshot<LocalStorageOptions> localStorageOptions) : IStorageClient
    {
        public StorageType StorageType => StorageType.Backup;
        private readonly LocalStorageOptions _localStorageOptions = localStorageOptions.Value;

        public async Task<Uri> SaveAsync(string key, Stream stream, CancellationToken cancellation = default)
        {
            if (string.IsNullOrWhiteSpace(_localStorageOptions.WorkingDirectory))
                throw new ApplicationException("工作目录不能为空");
            var fullPath = Path.Combine(_localStorageOptions.WorkingDirectory, key);
            var fullDir = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(fullDir))
                Directory.CreateDirectory(fullDir!);
            using var targetStream = File.OpenWrite(fullPath);
            await stream.CopyToAsync(targetStream, cancellation);
            return new Uri(fullPath);
        }
    }
}
