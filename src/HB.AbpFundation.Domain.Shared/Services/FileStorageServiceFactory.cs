

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Qiniu.Storage;

namespace HB.AbpFundation.Domain.Shared.Services
{
    public class FileStorageServiceFactory
    {
        private readonly IConfiguration _configuration;
        private readonly Dictionary<string, IFileStorageService> _services = new();

        public FileStorageServiceFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            InitializeServices();
        }

        private void InitializeServices()
        {
            // Initialize Qiniu service if configured
            var qiniuConfig = _configuration.GetSection("FileStorage:Qiniu");
            if (qiniuConfig.Exists())
            {
                _services.Add("qiniu", new QiniuStorageService(
                    qiniuConfig["AccessKey"],
                    qiniuConfig["SecretKey"],
                    qiniuConfig["Bucket"],
                    new Config { Zone = GetQiniuZone(qiniuConfig["Zone"]) }));
            }

            // Initialize Aliyun service if configured
            var aliyunConfig = _configuration.GetSection("FileStorage:Aliyun");
            if (aliyunConfig.Exists())
            {
                _services.Add("aliyun", new AliyunStorageService(
                    aliyunConfig["Endpoint"],
                    aliyunConfig["AccessKeyId"],
                    aliyunConfig["AccessKeySecret"],
                    aliyunConfig["BucketName"]));
            }

            // Initialize MinIO service if configured
            var minioConfig = _configuration.GetSection("FileStorage:MinIO");
            if (minioConfig.Exists())
            {
                _services.Add("minio", new MinioStorageService(
                    minioConfig["Endpoint"],
                    minioConfig["AccessKey"],
                    minioConfig["SecretKey"],
                    minioConfig["BucketName"],
                    bool.Parse(minioConfig["UseSSL"] ?? "false")));
            }
        }

        public IFileStorageService GetService(string storageType = "qiniu")
        {
            if (_services.TryGetValue(storageType, out var service))
            {
                return service;
            }
            throw new ArgumentException($"No storage service configured for type: {storageType}");
        }

        private static Zone GetQiniuZone(string zone)
        {
            return zone switch
            {
                "ZoneCnEast" => Zone.ZONE_CN_East,
                "ZoneCnNorth" => Zone.ZONE_CN_North,
                "ZoneCnSouth" => Zone.ZONE_CN_South,
                "ZoneUsNorth" => Zone.ZONE_US_North,
                _ => Zone.ZONE_CN_East
            };
        }
    }
}

