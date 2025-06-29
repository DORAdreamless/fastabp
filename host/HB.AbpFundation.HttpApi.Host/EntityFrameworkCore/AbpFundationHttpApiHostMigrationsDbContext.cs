using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace HB.AbpFundation.EntityFrameworkCore;

public class AbpFundationHttpApiHostMigrationsDbContext : AbpDbContext<AbpFundationHttpApiHostMigrationsDbContext>
{
    public AbpFundationHttpApiHostMigrationsDbContext(DbContextOptions<AbpFundationHttpApiHostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureAbpFundation();
    }
}
