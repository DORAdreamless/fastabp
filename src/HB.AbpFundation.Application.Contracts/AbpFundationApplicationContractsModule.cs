using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace HB.AbpFundation;

[DependsOn(
    typeof(AbpFundationDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class AbpFundationApplicationContractsModule : AbpModule
{

}
