using Volo.Abp;
using Volo.Abp.MongoDB;

namespace HB.AbpFundation.MongoDB;

public static class AbpFundationMongoDbContextExtensions
{
    public static void ConfigureAbpFundation(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
    }
}
