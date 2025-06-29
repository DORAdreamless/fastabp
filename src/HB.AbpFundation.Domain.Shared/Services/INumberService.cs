using System.Collections.Generic;
using System.Threading.Tasks;

namespace HB.AbpFundation.Services
{
    public interface INumberService
    {
        /// <summary>
        /// 获取账号
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<List<string>> GetUserNameListAsync(int count=1);
    }
}
