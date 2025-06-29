using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.DTOs.RBAC;
using HB.AbpFundation.Persistences;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HB.AbpFundation.Repositories.RBAC
{
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
}
