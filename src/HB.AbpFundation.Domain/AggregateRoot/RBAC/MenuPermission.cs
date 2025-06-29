using HB.AbpFundation.Persistences;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HB.AbpFundation.AggregateRoot.RBAC
{
    /// <summary>
    /// 菜单权限
    /// </summary>
    [Table("rbac_menu_permission")]
    public class MenuPermission: PersistenceObjectBase
    {
        /// <summary>
        /// 菜单编码
        /// </summary>
        public Guid MenuId { get; set; }

        /// <summary>
        /// 权限码
        /// </summary>
        [Required]
        [StringLength(20)]
        public string PermissionCode { get; set; }

        /// <summary>
        /// 权限名
        /// </summary>
        [Required]
        [StringLength(20)]
        public string PermissionName { get; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get; set; }
        /// <summary>
        /// 排序时间
        /// </summary>
        public DateTime SequenceTime { get; set; } = DateTime.Now;
    }


}
