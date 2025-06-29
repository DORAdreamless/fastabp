namespace HB.AbpFundation;

public static class AbpFundationDbProperties
{
    public static string DbTablePrefix { get; set; } = "AbpFundation";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "AbpFundation";
}
