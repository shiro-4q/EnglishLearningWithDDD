using FileService.Domain.Enums;

namespace FileService.Domain.Interfaces
{
    public interface IStorageClient
    {
        StorageType StorageType { get; }

        /// <summary>
        /// 存储上传文件
        /// </summary>
        /// <param name="key">存储路径</param>
        /// <param name="stream">文件流</param>
        /// <returns>存储文件的可访问路径</returns>
        Task<Uri> SaveAsync(string key, Stream stream);
    }
}
