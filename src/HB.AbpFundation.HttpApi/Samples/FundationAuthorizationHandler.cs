using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HB.AbpFundation.Context;
using Microsoft.AspNetCore.Authorization;

namespace HB.AbpFundation.Samples
{

    public class FundationAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IContextService _contextService;

        public FundationAuthorizationHandler(IContextService contextService)
        {
            _contextService = contextService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (!string.IsNullOrWhiteSpace(requirement.PermissionName))
            {
                // 获取用户权限列表
                var permissions = await _contextService.GetPermissionListAsync();

                // 检查权限
                if (permissions.Contains(requirement.PermissionName))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
           
        }
    }


}
