using System;
using System.Collections.Generic;

namespace HB.AbpFundation.DTOs.Common
{
    /// <summary>
    /// 查询单条数据
    /// </summary>
    public class GetIdInput: QueryInput
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }
    }
}
