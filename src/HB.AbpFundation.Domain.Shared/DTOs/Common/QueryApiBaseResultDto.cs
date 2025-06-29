using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.AbpFundation.DTOs.Common
{
    /// <summary>
    /// API封装
    /// </summary>
    public class QueryApiBaseResultDto
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 提示消息
        /// </summary>
        public string Msg { get; set; }
    }
    /// <summary>
    /// API封装
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryApiBaseResultDto<T>: QueryApiBaseResultDto
    {
        
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }
}
