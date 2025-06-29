using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace HB.AbpFundation.Samples
{
    public class FundationAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {

        public FundationAuthorizationPolicyProvider(
            IOptions<AuthorizationOptions> options) : base(options)
        {
        }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // 首先尝试从默认提供程序获取策略
            var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .AddRequirements(new PermissionRequirement(policyName))
                     .Build();

            return policy;
        }
    }


}
