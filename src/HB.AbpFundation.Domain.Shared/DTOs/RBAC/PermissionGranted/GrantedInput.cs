using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.AbpFundation.DTOs.RBAC
{
    public class GrantedInput
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
        public List<string> PermissionCodes { get; set; }=new List<string>();
    }
}
