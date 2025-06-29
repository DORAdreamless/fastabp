using HB.AbpFundation.DTOs.Common;
using System;

namespace HB.AbpFundation.DTOs.RBAC
{
    /// <summary>
    /// 角色
    /// </summary>
    public class RoleDto:AggregateDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
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

    public class GetRoleInput : GetListInput
    {

    }

 
}
