using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace HB.AbpFundation.MongoDB;

[DependsOn(
    typeof(AbpFundationApplicationTestModule),
    typeof(AbpFundationMongoDbModule)
)]
public class AbpFundationMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}
