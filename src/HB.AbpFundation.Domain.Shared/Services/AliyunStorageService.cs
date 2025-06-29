

using System;
using System.IO;
using System.Threading.Tasks;
using Aliyun.OSS;
using System.Web;

namespace HB.AbpFundation.Domain.Shared.Services
{
    public class AliyunStorageService : FileStorageServiceBase
    {
        private readonly OssClient _client;
        private readonly string _bucketName;

        public AliyunStorageService(string endpoint, string accessKeyId, string accessKeySecret, string bucketName)
        {
            _client = new OssClient(endpoint, accessKeyId, accessKeySecret);
            _bucketName = bucketName;
        }

        public override async Task<string> UploadAsync(Stream fileStream, string fileName, string storageType = "aliyun")
        {
            ValidateFileStream(fileStream);

            var key = GenerateFileKey(fileName);
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            var contentType = extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                _ => "application/octet-stream"
            };
            
            var metadata = new ObjectMetadata
            {
                ContentType = contentType
            };

            await Task.Run(() => _client.PutObject(_bucketName, key, fileStream, metadata));
            return key;
        }

        public override async Task<Stream> DownloadAsync(string fileKey, string storageType = "aliyun")
        {
            var memoryStream = new MemoryStream();
            var result = await Task.Run(() => _client.GetObject(_bucketName, fileKey));
            await result.Content.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }

        public override async Task DeleteAsync(string fileKey, string storageType = "aliyun")
        {
            await Task.Run(() => _client.DeleteObject(_bucketName, fileKey));
        }

        public override async Task<FileMetadata> GetMetadataAsync(string fileKey, string storageType = "aliyun")
        {
            var metadata = await Task.Run(() => _client.GetObjectMetadata(_bucketName, fileKey));
            return new FileMetadata
            {
                FileName = fileKey,
                Size = metadata.ContentLength,
                ContentType = metadata.ContentType,
                LastModified = metadata.LastModified
            };
        }
    }
}

