using HB.AbpFundation.DTOs.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.AbpFundation.DTOs.RBAC
{
    public class GetMenuInput:GetListInput
    {
        /// <summary>
        /// 提供者对应的ID（R=角色,U=用户,T=租户）
        /// </summary>
        public string ProviderKey { get; set; }
        /// <summary>
        /// 提供者（R=角色,U=用户,T=租户）
        /// </summary>
        public string ProviderName { get; set; } = "R";
    }
    public class CreateMenuInput
    {
        /// <summary>
        /// 上级菜单
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [StringLength(50)]
        public string Icon { get; set; }
        /// <summary>
        /// 访问地址
        /// </summary>
        [StringLength(100)]
        public string Url { get; set; }
        /// <summary>
        /// 组件
        /// </summary>
        [StringLength(50)]
        public string Component { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(50)]
        public string Remarks { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public List<MenuPermissionInput> Permissions { get; set; } = new List<MenuPermissionInput>();

    }

    public class UpdateMenuInput:GetIdInput
    {
        /// <summary>
        /// 上级菜单
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [StringLength(50)]
        public string Icon { get; set; }
        /// <summary>
        /// 访问地址
        /// </summary>
        [StringLength(100)]
        public string Url { get; set; }
        /// <summary>
        /// 组件
        /// </summary>
        [StringLength(50)]
        public string Component { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(50)]
        public string Remarks { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public List<MenuPermissionInput> Permissions { get; set; }=new List<MenuPermissionInput>();
    }

    public class MenuPermissionInput
    {
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
    }

    public class MenuDto:AggregateDto
    {
        /// <summary>
        /// 上级菜单
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 访问地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 组件
        /// </summary>
        public string Component { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 排序时间
        /// </summary>
        public DateTime SequenceTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public List<MenuPermissionInput> Permissions { get; set; }=new List<MenuPermissionInput>();
        /// <summary>
        /// 选择的权限
        /// </summary>
        public List<string> CheckPermissions { get; set; } = new List<string>();
        /// <summary>
        /// 是否全选
        /// </summary>
        public bool CheckPermissionAll { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<MenuDto> Children { get; set; } 

    }
}
