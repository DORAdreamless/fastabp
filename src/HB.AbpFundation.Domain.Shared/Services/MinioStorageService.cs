


using System;
using System.IO;
using System.Threading.Tasks;
using Minio;
using Minio.DataModel.Args;
using System.Net.Mime;

namespace HB.AbpFundation.Domain.Shared.Services
{
    public class MinioStorageService : FileStorageServiceBase
    {
        private readonly IMinioClient _client;
        private readonly string _bucketName;

        public MinioStorageService(string endpoint, string accessKey, string secretKey, string bucketName, bool secure = false)
        {
            _client = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .WithSSL(secure)
                .Build();
            _bucketName = bucketName;
        }

        public override async Task<string> UploadAsync(Stream fileStream, string fileName, string storageType = "minio")
        {
            ValidateFileStream(fileStream);

            var key = GenerateFileKey(fileName);
            var contentType = MediaTypeNames.Application.Octet; // Default content type

            await _client.PutObjectAsync(
                new PutObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(key)
                    .WithStreamData(fileStream)
                    .WithObjectSize(fileStream.Length)
                    .WithContentType(contentType));

            return key;
        }

        public override async Task<Stream> DownloadAsync(string fileKey, string storageType = "minio")
        {
            var memoryStream = new MemoryStream();

            await _client.GetObjectAsync(
                new GetObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileKey)
                    .WithCallbackStream(stream => stream.CopyTo(memoryStream)));

            memoryStream.Position = 0;
            return memoryStream;
        }

        public override async Task DeleteAsync(string fileKey, string storageType = "minio")
        {


            await _client.RemoveObjectAsync(
                new RemoveObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileKey));


        }

        public override async Task<FileMetadata> GetMetadataAsync(string fileKey, string storageType = "minio")
        {



            var stat = await _client.StatObjectAsync(
                new StatObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileKey));




            return new FileMetadata
            {
                FileName = fileKey,
                Size = stat.Size,
                ContentType = stat.ContentType,
                LastModified = stat.LastModified
            };
        }
    }
}


