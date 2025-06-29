using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace HB.AbpFundation.MongoDB;

[ConnectionStringName(AbpFundationDbProperties.ConnectionStringName)]
public interface IAbpFundationMongoDbContext : IAbpMongoDbContext
{
    /* Define mongo collections here. Example:
     * IMongoCollection<Question> Questions { get; }
     */
}
