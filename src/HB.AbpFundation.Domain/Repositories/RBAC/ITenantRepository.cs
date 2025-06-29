using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace HB.AbpFundation.Repositories.RBAC
{
    public interface IPermissionGrantedRepository:IRepository<PermissionGranted>
    {

    }

    public interface ITenantRepository:IRepository<Tenant>
    {

    }

    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userRoles"></param>
        /// <returns></returns>
        Task<bool> CreateAsync(User user, List<UserRole> userRoles);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<RoleDto>> GetUserRolesAsync(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userRoles"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(User user, List<UserRole> userRoles);
    }

    public interface IMenuRepository : IRepository<Menu>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="listMenuPermission"></param>
        /// <returns></returns>
        Task<bool> CreateAsync(Menu menu, List<MenuPermission> listMenuPermission);
        /// <summary>
        /// 获取所有菜单权限
        /// </summary>
        /// <returns></returns>
        Task<List<MenuPermission>> GetAllMenuPermissionsAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MenuDto> GetDetailAsync(GetIdInput input);
        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="listMenuPermission"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(Menu menu, List<MenuPermission> listMenuPermission);
    }

    /// <summary>
    /// 角色仓储
    /// </summary>
    public interface IRoleRepository : IRepository<Role>
    {

    }
}
