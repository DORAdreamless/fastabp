using HB.AbpFundation.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace HB.AbpFundation.Permissions;

public class AbpFundationPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AbpFundationPermissions.GroupName, L("Permission:AbpFundation"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpFundationResource>(name);
    }
}
