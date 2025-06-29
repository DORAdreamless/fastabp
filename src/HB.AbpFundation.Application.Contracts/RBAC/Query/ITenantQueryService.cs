using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.DTOs.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace HB.AbpFundation.RBAC
{
    /// <summary>
    /// 租户服务接口
    /// </summary>
    public interface ITenantQueryService : IApplicationService
    {
        /// <summary>
        /// 租户分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<TenantDto>> GetListAsync(GetTenantInput input);

        /// <summary>
        /// 租户查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TenantDto> GetAsync(GetIdInput input);
    }
}
