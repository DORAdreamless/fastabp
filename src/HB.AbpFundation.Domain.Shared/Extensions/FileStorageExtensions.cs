

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using HB.AbpFundation.Domain.Shared.Services;

namespace HB.AbpFundation.Domain.Shared.Extensions
{
    public static class FileStorageExtensions
    {
        public static IServiceCollection AddFileStorageServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register factory as singleton
            services.AddSingleton<FileStorageServiceFactory>();

            // Register interface that will resolve the default storage service
            services.AddScoped<IFileStorageService>(provider => 
            {
                var factory = provider.GetRequiredService<FileStorageServiceFactory>();
                var defaultProvider = configuration["FileStorage:DefaultProvider"] ?? "qiniu";
                return factory.GetService(defaultProvider);
            });

            return services;
        }
    }
}

