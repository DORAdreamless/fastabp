using HB.AbpFundation.Context;
using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.DTOs.RBAC;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace HB.AbpFundation.RBAC
{
    public interface IPermissionGrantedQueryService : IApplicationService
    {
        Task<ListResultDto<string>> GetUserGrantedAsync(string userId, List<string> roleIds);
    }
    public interface IUserQueryService : IApplicationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<UserDto> GetAsync(GetIdInput input);


        Task<PagedResultDto<UserDto>> GetListAsync(GetUserInput input);

        Task<ContextUser> GetContextUserAsync();
    }
    /// <summary>
    /// 角色查询服务
    /// </summary>
    public interface IRoleQueryService : IApplicationService
    {
        /// <summary>
        /// 查询所有可用的角色
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<RoleDto>> GetAllAsync();
        /// <summary>
        /// 角色列表查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<RoleDto>> GetListAsync(GetRoleInput input);
        /// <summary>
        /// 查询角色详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RoleDto> GetAsync(GetIdInput input);
      
    }

    public interface IMenuQueryService : IApplicationService
    {
        Task<MenuDto> GetAsync(GetIdInput input);


        Task<ListResultDto<MenuDto>> GetAllAsync(GetMenuInput input);
    }

    
}
