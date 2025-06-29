
using System;
using System.IO;
using System.Threading.Tasks;

namespace HB.AbpFundation.Domain.Shared.Services
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Upload file to storage
        /// </summary>
        Task<string> UploadAsync(Stream fileStream, string fileName, string storageType = null);

        /// <summary>
        /// Download file from storage
        /// </summary>
        Task<Stream> DownloadAsync(string fileKey, string storageType = null);

        /// <summary>
        /// Delete file from storage
        /// </summary>
        Task DeleteAsync(string fileKey, string storageType = null);

        /// <summary>
        /// Get file metadata
        /// </summary>
        Task<FileMetadata> GetMetadataAsync(string fileKey, string storageType = null);
    }

    public class FileMetadata
    {
        public string FileName { get; set; } = string.Empty;
        public long Size { get; set; }
        public string ContentType { get; set; } = "application/octet-stream";
        public DateTime LastModified { get; set; } = DateTime.MinValue;
    }
}
