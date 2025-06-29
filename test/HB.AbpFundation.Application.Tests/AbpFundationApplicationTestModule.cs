using Volo.Abp.Modularity;

namespace HB.AbpFundation;

[DependsOn(
    typeof(AbpFundationApplicationModule),
    typeof(AbpFundationDomainTestModule)
    )]
public class AbpFundationApplicationTestModule : AbpModule
{

}
