using HB.AbpFundation.Persistences;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HB.AbpFundation.AggregateRoot.RBAC
{
    /// <summary>
    /// 授权数据
    /// </summary>
    [Table("rbac_permission_granted")]
    public class PermissionGranted:PersistenceObjectBase
    {
        /// <summary>
        /// 提供者对应的ID（R=角色,U=用户,T=租户）
        /// </summary>
        [StringLength(50)]
        public string ProviderKey { get; set; }
        /// <summary>
        /// 提供者（R=角色,U=用户,T=租户）
        /// </summary>
        [StringLength(50)]
        public string ProviderName { get; set; } = "R";

        /// <summary>
        /// 权限码
        /// </summary>
        [StringLength(100)]
        public string PermissionCode { get; set; }
    }
}
