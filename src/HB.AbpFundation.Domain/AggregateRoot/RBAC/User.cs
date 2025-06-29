using HB.AbpFundation.Helpers;
using HB.AbpFundation.Persistences;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HB.AbpFundation.AggregateRoot.RBAC
{
    /// <summary>
    /// 用户
    /// </summary>
    [Table("rbac_user")]
    public class User : PersistenceObjectBase
    {
        /// <summary>
        /// 此用户的用户名。
        /// </summary>
        [StringLength(100)]
        public string UserName { get; set; } = string.Empty;


        /// <summary>
        /// 此用户的规范化用户名。
        /// </summary>
        [StringLength(100)]
        public string NormalizedUserName { get; set; } = string.Empty;

        /// <summary>
        /// 此用户的名称。
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 此用户的姓氏。
        /// </summary>
        [StringLength(50)]
        public string Surname { get; set; }

        /// <summary>
        /// 此用户的电子邮件地址。
        /// </summary>
        [StringLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// 此用户的规范化电子邮件地址。
        /// </summary>
        [StringLength(100)]
        public string NormalizedEmail { get; set; }

        /// <summary>
        /// 指示用户是否已确认其电子邮件地址。
        /// <value>如果电子邮件地址已确认，则为 true，否则为 false。</value>
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// 此用户的密码的加盐和哈希表示。
        /// </summary>
        [StringLength(100)]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// 一个随机值，每当用户的凭据更改时（密码更改、登录移除）必须更改。
        /// </summary>
        [StringLength(50)]
        public string SecurityStamp { get; set; } = string.Empty;


        /// <summary>
        /// 此用户的电话号码。
        /// </summary>
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 指示用户是否已确认其电话地址。
        /// <value>如果电话号码已确认，则为 true，否则为 false。</value>
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// 指示此用户是否处于活动状态。
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 指示是否为此用户启用了两步验证。
        /// <value>如果启用了两步验证，则为 true，否则为 false。</value>
        /// </summary>
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// 在 UTC 中，任何用户锁定结束的日期和时间。
        /// <remarks>过去的时间值表示用户未被锁定。</remarks>
        /// </summary>
        public DateTimeOffset? LockoutEnd { get; set; }

        /// <summary>
        /// 指示用户是否可能被锁定。
        /// <value>如果用户可能被锁定，则为 true，否则为 false。</value>
        /// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// 当前用户的失败登录尝试次数。
        /// </summary>
        public int AccessFailedCount { get; set; }

        /// <summary>
        /// 指示用户是否应在下次登录时更改密码。
        /// </summary>
        public bool ShouldChangePasswordOnNextLogin { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [StringLength(200)]
        public string Avatar {  get; set; }

        /// <summary>
        /// 个人简介
        /// </summary>
        [StringLength(500)]
        public string Introduction { get; set; }




        public void SetPassword(string password)
        {
            this.SecurityStamp = Guid.NewGuid().ToString("N");
            this.PasswordHash = PasswordHelper.GetPasswordHash(password, this.SecurityStamp);
        }

        public void LoginFail()
        {
            this.AccessFailedCount++;
            if (this.AccessFailedCount > 5)
            {
                this.LockoutEnabled = true;
                this.LockoutEnd = DateTime.Now.AddMinutes(15);
            }
        }

        public void LoginSuccess()
        {
            this.AccessFailedCount = 0;
            this.LockoutEnabled = false;
        }

        public void SetUserName(string userName)
        {
            this.UserName = userName;
            this.NormalizedUserName = userName.ToUpper();
        }

        public void SetEmail(string email)
        {
            this.Email = email;
            this.NormalizedEmail = email.ToUpper();
        }
    }
}
