using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace HB.AbpFundation.MongoDB;

[ConnectionStringName(AbpFundationDbProperties.ConnectionStringName)]
public class AbpFundationMongoDbContext : AbpMongoDbContext, IAbpFundationMongoDbContext
{
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureAbpFundation();
    }
}
