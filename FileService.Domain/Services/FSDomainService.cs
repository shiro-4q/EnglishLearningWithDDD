using FileService.Domain.Entities;
using FileService.Domain.Enums;
using FileService.Domain.Interfaces;
using FileService.Domain.Repositories;
using Q.Commons.Helpers;

namespace FileService.Domain.Services
{
    public class FSDomainService(IFSRepository repository, IEnumerable<IStorageClient> storageClients)
    {
        private readonly IFSRepository _repository = repository;
        //用这种方式可以解决内置DI不能使用名字注入不同实例的问题
        private readonly IStorageClient _backupStorage = storageClients.First(s => s.StorageType == StorageType.Backup);
        private readonly IStorageClient _remoteStorage = storageClients.First(s => s.StorageType == StorageType.Remote);

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="cancellation">取消</param>
        /// <returns></returns>
        public async Task<UploadItem> UploadAsync(Stream stream, string fileName, CancellationToken cancellation = default)
        {
            var fileSize = stream.Length;
            var fileHash = HashHelper.ComputeSha256Hash(stream);
            var uploadItem = await _repository.FindAsync(fileSize, fileHash);
            if (uploadItem != null)
                return uploadItem;
            var filePath = FileHelper.GenerateFilePathWithHash(fileHash, fileName);
            stream.Position = 0;
            var backupUrl = await _backupStorage.SaveAsync(filePath, stream, cancellation);
            stream.Position = 0;
            var remoteUrl = await _remoteStorage.SaveAsync(filePath, stream, cancellation);
            uploadItem = new UploadItem(fileName, fileSize, fileHash, backupUrl, remoteUrl);
            await _repository.AddAsync(uploadItem);
            return uploadItem;
        }
    }
}
