using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace HB.AbpFundation.EntityFrameworkCore;

[ConnectionStringName(AbpFundationDbProperties.ConnectionStringName)]
public interface IAbpFundationDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
