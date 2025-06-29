using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HB.AbpFundation.DTOs.RBAC
{
    public class CreateUserInput
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
        public List<Guid> RoleIds { get; set; }=new List<Guid>();
    }



    public class UpdateUserProfileInput
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        //个人简介
        [StringLength(500)]
        public string Introduction { get; set; }

 
    }

    public class UpdatePasswordInput:IValidatableObject
    {
      //  currentPassword: '',
      //newPassword: '',
      //confirmPassword: '',

        [Required]
        [StringLength(50)]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(50)]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(50)]
        public string ConfirmPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NewPassword != ConfirmPassword)
            {
                yield return new ValidationResult("新密码和确认密码不一致");
            }
        }
    }
}
