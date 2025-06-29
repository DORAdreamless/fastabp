using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.DTOs.Common;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.Persistences;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HB.AbpFundation.Repositories.RBAC
{
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
}
