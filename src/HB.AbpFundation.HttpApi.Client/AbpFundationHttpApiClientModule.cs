using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace HB.AbpFundation;

[DependsOn(
    typeof(AbpFundationApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AbpFundationHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(AbpFundationApplicationContractsModule).Assembly,
            AbpFundationRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpFundationHttpApiClientModule>();
        });

    }
}
