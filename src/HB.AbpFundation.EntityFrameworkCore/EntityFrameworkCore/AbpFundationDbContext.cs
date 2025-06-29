using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace HB.AbpFundation.EntityFrameworkCore;

[ConnectionStringName(AbpFundationDbProperties.ConnectionStringName)]
public class AbpFundationDbContext : AbpDbContext<AbpFundationDbContext>, IAbpFundationDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public AbpFundationDbContext(DbContextOptions<AbpFundationDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureAbpFundation();
    }
}
