
using System;
using System.IO;
using System.Threading.Tasks;

namespace HB.AbpFundation.Domain.Shared.Services
{
    public abstract class FileStorageServiceBase : IFileStorageService
    {
        public abstract Task<string> UploadAsync(Stream fileStream, string fileName, string storageType);
        public abstract Task<Stream> DownloadAsync(string fileKey, string storageType);
        public abstract Task DeleteAsync(string fileKey, string storageType);
        public abstract Task<FileMetadata> GetMetadataAsync(string fileKey, string storageType);

        protected virtual void ValidateFileStream(Stream fileStream)
        {
            if (fileStream == null)
                throw new ArgumentNullException(nameof(fileStream));
            
            if (!fileStream.CanRead)
                throw new ArgumentException("Stream must be readable", nameof(fileStream));
        }

        protected virtual string GenerateFileKey(string fileName)
        {
            return $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        }
    }
}
