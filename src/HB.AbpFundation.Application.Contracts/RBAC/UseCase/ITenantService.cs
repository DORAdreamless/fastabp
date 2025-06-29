using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.DTOs.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace HB.AbpFundation.RBAC.UseCase
{
    public interface IPermissionGrantedService : IApplicationService
    {
        Task<bool> GrantedAsync(GrantedInput input);
    }
    /// <summary>
    /// 角色服务
    /// </summary>
    public interface IRoleService: IApplicationService
    {
      
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> CreateAsync(CreateRoleInput input);
        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(UpdateRoleInput input);
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid id);
    }
    public interface IUserService : IApplicationService
    {
        Task<bool> CreateAsync(CreateUserInput input);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// 账号登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<JWTToken> LoginAsync(LoginInput input);
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(UpdateUserInput input);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> UpdatePasswordAsync(UpdatePasswordInput input);

        /// <summary>
        /// 修改个人资料
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> UpdateProfileAsync(UpdateUserProfileInput input);
    }
    /// <summary>
    /// 菜单服务
    /// </summary>
    public interface IMenuService: IApplicationService
    {
        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> CreateAsync(CreateMenuInput input);
        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(UpdateMenuInput input);
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid id);

    }
    /// <summary>
    /// 租户服务接口
    /// </summary>
    public interface ITenantService:IApplicationService
    {
        /// <summary>
        /// 新增租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> CreateAsync(CreateTenantInput input);
        /// <summary>
        /// 更新租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(UpdateTenantInput input);
        /// <summary>
        /// 删除租户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid id);

    }
}
