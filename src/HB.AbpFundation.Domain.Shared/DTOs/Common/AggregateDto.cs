using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace HB.AbpFundation.DTOs.Common
{
    /// <summary>
    /// 聚合根DTO
    /// </summary>
    public abstract class AggregateDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public DateTime CreationTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 创建人
        /// </summary>
        public Guid? CreatorId { get; set; }
        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreatorName { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        public Guid? LastModifierId { get; set; }
        /// <summary>
        /// 最后修改人姓名
        /// </summary>
        public string LastModifierName { get; set; }
       
    }
}
