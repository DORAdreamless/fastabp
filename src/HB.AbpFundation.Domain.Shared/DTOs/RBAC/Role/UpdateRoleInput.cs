using HB.AbpFundation.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace HB.AbpFundation.DTOs.RBAC
{
    /// <summary>
    /// 更新角色
    /// </summary>
    public class UpdateRoleInput:GetIdInput
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
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}
