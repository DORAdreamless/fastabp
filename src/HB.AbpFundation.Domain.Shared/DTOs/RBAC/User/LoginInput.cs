using System.ComponentModel.DataAnnotations;

namespace HB.AbpFundation.DTOs.RBAC
{
    public class LoginInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
