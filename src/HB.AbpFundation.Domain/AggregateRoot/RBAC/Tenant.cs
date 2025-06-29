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
    /// 租户
    /// </summary>
    [Table("rbac_tenant")]
    public class Tenant:PersistenceObjectBase
    {
        /// <summary>
        /// 租户名称
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        [StringLength(200)]
        public string MasterConnectionString { get; set; }

        /// <summary>
        /// 读库连接字符串
        /// </summary>
        [StringLength(2000)]
        public string ReadOnlyConnectionStrings { get; set; }
        /// <summary>
        /// logo
        /// </summary>
        [StringLength(100)]
        public string Logo { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [StringLength(100)]
        public string ContractName { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [StringLength(100)]
        public string ContractPhone { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}
