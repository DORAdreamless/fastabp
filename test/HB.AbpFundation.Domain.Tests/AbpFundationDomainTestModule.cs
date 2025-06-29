using Volo.Abp.Modularity;

namespace HB.AbpFundation;

[DependsOn(
    typeof(AbpFundationDomainModule),
    typeof(AbpFundationTestBaseModule)
)]
public class AbpFundationDomainTestModule : AbpModule
{

}
