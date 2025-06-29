using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace HB.AbpFundation;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpFundationInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpFundationInstallerModule>();
        });
    }
}
