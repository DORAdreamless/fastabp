using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace HB.AbpFundation.EntityFrameworkCore;

[DependsOn(
    typeof(AbpFundationDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class AbpFundationEntityFrameworkCoreModule : AbpModule
{
    

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<AbpFundationDbContext>(options =>
        {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
        });

       
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        base.OnApplicationInitialization(context);


    }
}


public class D
{

}