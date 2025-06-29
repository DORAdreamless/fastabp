using System;
using System.Collections.Generic;
using HB.AbpFundation.DTOs.Common;

namespace HB.AbpFundation.DTOs.RBAC
{
    public class UpdateUserInput:GetIdInput
    {
        /// <summary>
        /// 此用户的姓氏。
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// 此用户的电子邮件地址。
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 此用户的电话号码。
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 指示此用户是否处于活动状态。
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 角色
        /// </summary>
        public List<Guid> RoleIds { get; set; } = new List<Guid>();
    }
}
