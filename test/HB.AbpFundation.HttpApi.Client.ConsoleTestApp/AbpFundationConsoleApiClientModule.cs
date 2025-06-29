using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace HB.AbpFundation;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpFundationHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class AbpFundationConsoleApiClientModule : AbpModule
{

}
