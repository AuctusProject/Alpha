using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Auctus.Util
{
    public class Util
    {
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime;
        }

        public static DateTime UnixMillisecondsTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp);
            return dtDateTime;
        }

        public static Int64 DatetimeToUnixSeconds(DateTime datetime)
        {
            return (Int64)(datetime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static Int64 DatetimeToUnixMilliseconds(DateTime datetime)
        {
            return (Int64)(datetime.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
        }

        public static double ConvertMonthlyToDailyRate(double monthlyRate)
        {
            return Math.Pow((monthlyRate / 100.0) + 1.0, 1.0 / 30.0) - 1.0;
        }
    }
}
