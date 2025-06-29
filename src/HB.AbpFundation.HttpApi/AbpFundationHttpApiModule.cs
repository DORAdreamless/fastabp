using Localization.Resources.AbpUi;
using HB.AbpFundation.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using HB.AbpFundation.Samples;
using Microsoft.AspNetCore.Authorization;

namespace HB.AbpFundation;

[DependsOn(
    typeof(AbpFundationApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpFundationHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpFundationHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpFundationResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });

        // 替换默认的IAuthorizationPolicyProvider
        context.Services.AddSingleton<IAuthorizationPolicyProvider, FundationAuthorizationPolicyProvider>();

        // 注册权限处理器
        context.Services.AddSingleton<IAuthorizationHandler, FundationAuthorizationHandler>();
    }
}
