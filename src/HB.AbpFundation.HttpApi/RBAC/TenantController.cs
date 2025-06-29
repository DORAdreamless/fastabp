using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.RBAC.UseCase;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace HB.AbpFundation.RBAC
{
    /// <summary>
    /// 租户接口
    /// </summary>
    [Route("api/AbpFundation/[controller]/[action]")]
    public class TenantController : AbpFundationController
    {
        private readonly ITenantQueryService _tenantQueryService;
        private readonly ITenantService _tenantService;

        public TenantController(ITenantQueryService tenantQueryService, ITenantService tenantService)
        {
            _tenantQueryService = tenantQueryService;
            _tenantService = tenantService;
        }

        /// <summary>
        /// 新增租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<QueryApiBaseResultDto<bool>> CreateAsync([FromBody]CreateTenantInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _tenantService.CreateAsync(input);
            });
        }
        /// <summary>
        /// 修改租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<QueryApiBaseResultDto<bool>> UpdateAsync([FromBody]UpdateTenantInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _tenantService.UpdateAsync(input);
            });
        }

        /// <summary>
        /// 删除租户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<QueryApiBaseResultDto<bool>> DeleteAsync(Guid id)
        {
            return await HandleAsync(async () =>
            {
                return await _tenantService.DeleteAsync(id);
            });
        }
        /// <summary>
        /// 查询租户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<QueryApiBaseResultDto<PagedResultDto<TenantDto>>> GetListAsync([FromQuery] GetTenantInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _tenantQueryService.GetListAsync(input);
            });
        }

        /// <summary>
        /// 查询租户详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<QueryApiBaseResultDto<TenantDto>> GetAsync([FromQuery]GetIdInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _tenantQueryService.GetAsync(input);
            });
        }
    }
}
