using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.AbpFundation.ValueObject
{
    public  class RedisKeys
    {
       
        /// <summary>
        /// 刷新令牌
        /// </summary>
        public const string REFRESH_TOKEN = "REFRESH_TOKEN";
        /// <summary>
        /// 上下文用户
        /// </summary>
        public const string CONTEXT_USER = "CONTEXT_USER";
    }
}
