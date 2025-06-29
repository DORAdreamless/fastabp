using System.Threading.Tasks;
using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.RBAC.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace HB.AbpFundation.RBAC
{
    [Route("api/AbpFundation/[controller]/[action]")]
    public class PermissionGrantedController : AbpFundationController
    {
        private readonly IPermissionGrantedService _permissionGrantedService;

        public PermissionGrantedController(IPermissionGrantedService permissionGrantedService)
        {
            _permissionGrantedService = permissionGrantedService;
        }

        [HttpPost]
        public async Task<QueryApiBaseResultDto<bool>> GrantedAsync([FromBody] GrantedInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _permissionGrantedService.GrantedAsync(input);
            });
        }
    }
}
