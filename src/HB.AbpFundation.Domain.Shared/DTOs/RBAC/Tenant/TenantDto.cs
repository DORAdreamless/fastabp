using HB.AbpFundation.DTOs.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace HB.AbpFundation.DTOs.RBAC
{
    public class TenantDto: AggregateDto
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
