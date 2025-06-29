using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace HB.AbpFundation.EntityFrameworkCore;

public static class AbpFundationDbContextModelCreatingExtensions
{
    public static void ConfigureAbpFundation(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(AbpFundationDbProperties.DbTablePrefix + "Questions", AbpFundationDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */
    }
}
