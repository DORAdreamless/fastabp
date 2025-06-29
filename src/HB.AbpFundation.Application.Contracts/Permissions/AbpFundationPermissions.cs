using Volo.Abp.Reflection;

namespace HB.AbpFundation.Permissions;

public class AbpFundationPermissions
{
    public const string GroupName = "AbpFundation";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(AbpFundationPermissions));
    }
}
