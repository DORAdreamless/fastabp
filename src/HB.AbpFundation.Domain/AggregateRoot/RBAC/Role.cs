using HB.AbpFundation.Persistences;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HB.AbpFundation.AggregateRoot.RBAC
{
    /// <summary>
    /// 角色
    /// </summary>
    [Table("rbac_role")]
    public class Role : PersistenceObjectBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public DateTime SenquenceTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 是否默认角色。
        /// </summary>
        public bool IsDefault { get; set; }
        /// <summary>
        /// 是否静态角色。
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// 是否公共角色。
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public bool Enabled { get; set; }
    }


}
