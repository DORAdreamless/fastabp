using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using HB.AbpFundation.DTOs.Common;
using SixLabors.ImageSharp;

namespace HB.AbpFundation;

[DependsOn(
    typeof(AbpFundationDomainModule),
    typeof(AbpFundationApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class AbpFundationApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        context.Services.AddAutoMapperObjectMapper<AbpFundationApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AbpFundationApplicationModule>(validate: true);
        });

        context.Services.Configure<AppSettings>(configuration.GetSection("App"));
    }
}
