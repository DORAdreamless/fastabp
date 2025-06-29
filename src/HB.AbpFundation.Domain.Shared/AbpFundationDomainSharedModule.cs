using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using HB.AbpFundation.Localization;
using Volo.Abp.Domain;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using HB.AbpFundation.Context;

namespace HB.AbpFundation;

[DependsOn(
    typeof(AbpValidationModule),
    typeof(AbpDddDomainSharedModule)
)]
[DependsOn(typeof(AbpCachingStackExchangeRedisModule))]
    public class AbpFundationDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpFundationDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpFundationResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/AbpFundation");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("AbpFundation", typeof(AbpFundationResource));
        });

        context.Services.AddTransient<IJwtTokenService, JwtTokenService>();
        context.Services.AddScoped<IContextService, ContextService>();
        
    }
}
