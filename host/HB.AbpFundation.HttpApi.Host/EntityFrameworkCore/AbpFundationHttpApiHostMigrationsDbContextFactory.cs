using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HB.AbpFundation.EntityFrameworkCore;

public class AbpFundationHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<AbpFundationHttpApiHostMigrationsDbContext>
{
    public AbpFundationHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<AbpFundationHttpApiHostMigrationsDbContext>()
            .UseMySql(configuration.GetConnectionString("Default"),MySqlServerVersion.LatestSupportedServerVersion);

        return new AbpFundationHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
