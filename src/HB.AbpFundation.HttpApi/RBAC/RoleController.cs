using System;
using System.Threading.Tasks;
using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.RBAC.UseCase;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace HB.AbpFundation.RBAC
{
    /// <summary>
    /// 角色接口
    /// </summary>
    [Route("api/AbpFundation/[controller]/[action]")]
    public class RoleController : AbpFundationController
    {
        private readonly IRoleQueryService _roleQueryService;
        private readonly IRoleService _roleService;

        public RoleController(IRoleQueryService roleQueryService, IRoleService roleService)
        {
            _roleQueryService = roleQueryService;
            _roleService = roleService;
        }
        /// <summary>
        /// 角色列表查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<QueryApiBaseResultDto<ListResultDto<RoleDto>>> GetAllAsync()
        {
            return await HandleAsync(async () =>
            {
                return await _roleQueryService.GetAllAsync();
            });
        }
        /// <summary>
        /// 角色列表查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<QueryApiBaseResultDto<PagedResultDto<RoleDto>>> GetListAsync([FromQuery]GetRoleInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _roleQueryService.GetListAsync(input);
            });
        }
        /// <summary>
        /// 查询角色详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<QueryApiBaseResultDto<RoleDto>> GetAsync([FromQuery] GetIdInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _roleQueryService.GetAsync(input);
            });
        }
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<QueryApiBaseResultDto<bool>> CreateAsync([FromBody] CreateRoleInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _roleService.CreateAsync(input);
            });
        }
        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<QueryApiBaseResultDto<bool>> UpdateAsync([FromBody] UpdateRoleInput input)
        {
            return await HandleAsync(async () =>
            {
                return await _roleService.UpdateAsync(input);
            });
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<QueryApiBaseResultDto<bool>> DeleteAsync(Guid id)
        {
            return await HandleAsync(async () =>
            {
                return await _roleService.DeleteAsync(id);
            });
        }
    }
}
