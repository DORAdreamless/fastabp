using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace HB.AbpFundation;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpFundationDomainSharedModule)
)]
public class AbpFundationDomainModule : AbpModule
{

}
