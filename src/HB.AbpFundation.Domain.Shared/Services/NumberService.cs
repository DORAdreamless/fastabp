using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.DependencyInjection;

namespace HB.AbpFundation.Services
{
    public class NumberService : INumberService, IScopedDependency
    {
        const string NUMBER_KEY = "NUMBER";
        const string USER_NAME_KEY = "USER_NAME";

        private readonly ConnectionMultiplexer _redis;

        public NumberService(ConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<List<string>> GetUserNameListAsync(int count = 1)
        {
            IDatabase db = _redis.GetDatabase();
            var value =await db.HashIncrementAsync(NUMBER_KEY,USER_NAME_KEY);
            List<string> list = new List<string>();
            for (int i = 0; i < count; i++)
            {
               var userName = DateTime.Now.ToString("yyMMdd")+(value+1).ToString().PadLeft(6, '0');
                list.Add(userName);
            }
            return list;
        }
    }
}
