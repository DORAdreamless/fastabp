using HB.AbpFundation.DTOs.Common;

namespace HB.AbpFundation.DTOs.RBAC
{
    /// <summary>
    /// 修改租户参数
    /// </summary>
    public class UpdateTenantInput:GetIdInput
    {
        /// <summary>
        /// 租户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
   
        /// <summary>
        /// logo
        /// </summary>
        public string Logo { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ContractName { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContractPhone { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}
