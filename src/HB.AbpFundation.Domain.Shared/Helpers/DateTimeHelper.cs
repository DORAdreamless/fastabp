using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB.AbpFundation.Helpers
{
    public static class DateTimeHelper
    {
        //
        // 摘要:
        //     Unix起始时间
        public static readonly DateTimeOffset BaseTime = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        //
        // 摘要:
        //     转换微信DateTime时间到C#时间
        //
        // 参数:
        //   dateTimeFromXml:
        //     微信DateTime
        public static DateTime GetDateTimeFromXml(long dateTimeFromXml)
        {
            return GetDateTimeOffsetFromXml(dateTimeFromXml).LocalDateTime;
        }

        //
        // 摘要:
        //     转换微信DateTime时间到C#时间
        //
        // 参数:
        //   dateTimeFromXml:
        //     微信DateTime
        public static DateTime GetDateTimeFromXml(string dateTimeFromXml)
        {
            return GetDateTimeFromXml(long.Parse(dateTimeFromXml));
        }

        //
        // 摘要:
        //     转换微信DateTimeOffset时间到C#时间
        //
        // 参数:
        //   dateTimeFromXml:
        //     微信DateTime
        public static DateTimeOffset GetDateTimeOffsetFromXml(long dateTimeFromXml)
        {
            return DateTimeOffset.FromUnixTimeSeconds(dateTimeFromXml);
        }

        //
        // 摘要:
        //     转换微信DateTimeOffset时间到C#时间
        //
        // 参数:
        //   dateTimeFromXml:
        //     微信DateTime
        public static DateTimeOffset GetDateTimeOffsetFromXml(string dateTimeFromXml)
        {
            return GetDateTimeFromXml(long.Parse(dateTimeFromXml));
        }


        //
        // 摘要:
        //     获取Unix时间戳
        //
        // 参数:
        //   dateTime:
        public static long GetUnixDateTime(DateTimeOffset dateTime)
        {
            return (long)(dateTime - BaseTime).TotalSeconds;
        }

        //
        // 摘要:
        //     获取Unix时间戳
        //
        // 参数:
        //   dateTime:
        public static long GetUnixDateTime(DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - BaseTime).TotalSeconds;
        }

        ////
        //// 摘要:
        ////     自动等待
        ////
        //// 参数:
        ////   waitingTime:
        ////     总共等待时间
        ////
        ////   waitingInterval:
        ////     每次等待间隔
        ////
        ////   work:
        ////     每次等待之前执行的方法（可为空）
        //public static async Task WaitingFor(TimeSpan waitingTime, TimeSpan waitingInterval, Action work = null)
        //{
        //    DateTimeOffset startTime = SystemTime.Now;
        //    do
        //    {
        //        work?.Invoke();
        //        await Task.Delay(waitingInterval);
        //    }
        //    while (!(SystemTime.NowDiff(startTime) >= waitingTime));
        //}
    }
}
