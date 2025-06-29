using HB.AbpFundation.Persistences;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HB.AbpFundation.AggregateRoot.RBAC
{
    /// <summary>
    /// 用户角色
    /// </summary>
    [Table("rbac_userroles")]
    public class UserRole : PersistenceObjectBase
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; set; }
    }

}
