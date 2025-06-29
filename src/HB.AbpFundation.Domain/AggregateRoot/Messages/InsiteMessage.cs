using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HB.AbpFundation.Persistences;

namespace HB.AbpFundation.AggregateRoot.Messages
{
    /// <summary>
    /// 站内信
    /// </summary>
    [Table("message_insite_message")]
    public class InsiteMessage: PersistenceObjectBase
    {
       
        /// <summary>
        /// 站内信标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 站内信内容
        /// </summary>
        public string Content { get;set; }
        /// <summary>
        /// 发送人
        /// </summary>
        public Guid SenderId { get; set; }

        /// <summary>
        /// 发送者
        /// </summary>
        public string SenderName { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 回复人
        /// </summary>
        public DateTime? ReplyUserId { get; set; }
        /// <summary>
        /// 回复人
        /// </summary>
        public string ReplyUserName { get; set; }
        /// <summary>
        /// 回复时间
        /// </summary>
        public DateTime? ReplyTime { get; set; }
    }
    /// <summary>
    /// 站内信回复
    /// </summary>
    [Table("message_insite_message_reply")]
    public class InsiteMessageReply: PersistenceObjectBase
    {

   

        /// <summary>
        /// 回复人
        /// </summary>
        public DateTime ReplyUserId { get; set; }
        /// <summary>
        /// 回复人
        /// </summary>
        [StringLength(50)]
        public string ReplyUserName { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        [StringLength(1000)]
        public string ReplyContent { get; set; }
        /// <summary>
        /// 回复时间
        /// </summary>
        public DateTime ReplyTime { get; set; }
    }
}
