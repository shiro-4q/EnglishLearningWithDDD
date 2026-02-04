using FileService.Domain.Enums;
using FileService.Domain.Interfaces;
using FileService.Domain.Repositories;

namespace FileService.Domain.Services
{
    public class FileDomainService(IFileRepository fileRepository, IEnumerable<IStorageClient> storageClients)
    {
        private readonly IFileRepository _fileRepository = fileRepository;
        private readonly IStorageClient _backupStorage = storageClients.First(s => s.StorageType == StorageType.Backup);
        private readonly IStorageClient _remoteStorage = storageClients.First(s => s.StorageType == StorageType.Remote);

        public async Task SaveFileAsync(Stream stream, string fileName)
        {
            //stream.
            //_fileRepository.FindAsync();
        }
    }
}
