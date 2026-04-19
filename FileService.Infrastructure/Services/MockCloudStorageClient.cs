using FileService.Domain.Enums;
using FileService.Domain.Interfaces;
using Q.Commons;

namespace FileService.Infrastructure.Services
{
    /// <summary>
    /// 存储文件到wwwroot目录的存储客户端，主要用于测试和开发环境，生产环境不应该使用它
    /// </summary>
    public class MockCloudStorageClient : IStorageClient
    {
        public StorageType StorageType => StorageType.Remote;

        public async Task<Uri> SaveAsync(string key, Stream stream, CancellationToken cancellation = default)
        {
            var fullPath = Path.Combine(BaseConfig.WebRootPath, key);
            var fullDir = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(fullDir))
                Directory.CreateDirectory(fullDir!);
            using var targetStream = File.OpenWrite(fullPath);
            await stream.CopyToAsync(targetStream, cancellation);
            return new Uri(fullPath);
        }
    }
}
