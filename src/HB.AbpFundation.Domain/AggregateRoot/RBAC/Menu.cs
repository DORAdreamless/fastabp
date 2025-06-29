using HB.AbpFundation.Persistences;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.AbpFundation.AggregateRoot.RBAC
{
    /// <summary>
    /// 菜单
    /// </summary>
    [Table("rbac_menu")]
    public class Menu: PersistenceObjectBase
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
        /// 排序时间
        /// </summary>
        public DateTime SequenceTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get;set; }
    }


}
