

using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using Qiniu.Storage;
using Qiniu.Util;

namespace HB.AbpFundation.Domain.Shared.Services
{
    public class QiniuStorageService : FileStorageServiceBase
    {
        private readonly Mac _mac;
        private readonly Config _config;
        private readonly string _bucket;
        private readonly BucketManager _bucketManager;
        private readonly UploadManager _uploadManager;

        public QiniuStorageService(string accessKey, string secretKey, string bucket, Config config = null)
        {
            _mac = new Mac(accessKey, secretKey);
            _bucket = bucket;
            _config = config ?? new Config();
            _bucketManager = new BucketManager(_mac, _config);
            _uploadManager = new UploadManager(_config);
        }

        public override async Task<string> UploadAsync(Stream fileStream, string fileName, string storageType = "qiniu")
        {
            ValidateFileStream(fileStream);

            var key = GenerateFileKey(fileName);
            var putPolicy = new PutPolicy { Scope = _bucket };
            var token = Auth.CreateUploadToken(_mac, putPolicy.ToJsonString());

            return await Task.Run(() => 
            {
                var result = _uploadManager.UploadStream(fileStream, key, token, null);
                if (result.Code != 200)
                    throw new Exception($"Qiniu upload failed: {result.Text}");
                return key;
            });
        }

        public override async Task<Stream> DownloadAsync(string fileKey, string storageType = "qiniu")
        {

            var memoryStream = new MemoryStream();
            var privateUrl = $"{_config.Zone.RsHost}/getfile/{_mac.AccessKey}/{_bucket}/{fileKey}?e={DateTimeOffset.UtcNow.AddSeconds(3600).ToUnixTimeSeconds()}&token={Auth.CreateDownloadToken(_mac, $"{_bucket}:{fileKey}")}";
            using var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(privateUrl);
            await response.Content.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }

        public override async Task DeleteAsync(string fileKey, string storageType = "qiniu")
        {
            await Task.Run(() => _bucketManager.Delete(_bucket, fileKey));
        }

        public override async Task<FileMetadata> GetMetadataAsync(string fileKey, string storageType = "qiniu")
        {
            var result = await Task.Run(() => _bucketManager.Stat(_bucket, fileKey));
            if (result.Code != 200)
                throw new Exception($"Failed to get metadata: {result.Text}");

            return new FileMetadata
            {
                FileName = fileKey,
                Size = result.Result.Fsize,
                ContentType = result.Result.MimeType ?? "application/octet-stream",
                LastModified = DateTimeOffset.FromUnixTimeSeconds(result.Result.PutTime / 10000000).DateTime
            };
        }
    }
}

