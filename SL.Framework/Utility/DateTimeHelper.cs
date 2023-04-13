using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.Framework.Utility
{
    /// <summary>
    /// DateTime Helper
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// UTC -> 국가별 시간으로 변환하기
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="zone"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ConvertTimeFromUtc(this DateTime dt, string zone = "Korea Standard Time", string format = "yyyy-MM-dd HH:mm:ss")
        {
            string returnValue = "";
            try
            {
                  TimeZoneInfo southKoreaZone = TimeZoneInfo.FindSystemTimeZoneById(zone);
                DateTime utcTime = dt; // an UTC DateTime                  
                DateTime pacific = TimeZoneInfo.ConvertTimeFromUtc(utcTime, southKoreaZone);

                returnValue = pacific.ToString(format);
            }
            catch (Exception)
            {
                returnValue = "1900-01-01 00:00:00";
            }
            return returnValue;
        }

        /// <summary>
        /// 2012-09-17T15:02:51.4021600-07:00 형태로 나오게 한다.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ConvertTimeFromUtc(this DateTime dt)
        {
            string returnValue = "";
            try
            {
                DateTime localTime = dt;
                return localTime.ToString("yyyy-MM-ddTHH:mm:ssK");
            }
            catch (Exception)
            {
                returnValue = "1900-01-01 00:00:00";
            }
            return returnValue;
        }

        /// <summary>
        /// UTC -> 국가별 시간으로 변환하기
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="zone"></param>
        /// <returns></returns>
        public static DateTime ConvertTimeFromUtcByDateTime(this DateTime dt, string zone = "Korea Standard Time")
        {
            try
            {
                TimeZoneInfo southKoreaZone = TimeZoneInfo.FindSystemTimeZoneById(zone);
                DateTime utcTime = dt; // an UTC DateTime                  
                return TimeZoneInfo.ConvertTimeFromUtc(utcTime, southKoreaZone);
            }
            catch (Exception)
            {
                return Convert.ToDateTime("1900-01-01 00:00:00");
            }
        }

        /// <summary>
        /// UTC -> 국가별 시간으로 변환하기
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="zone"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ConvertTimeFromUtcByString(this string dt, string zone = "Korea Standard Time", string format = "yyyy-MM-dd HH:mm:ss")
        {
            return ConvertTimeFromUtc(Convert.ToDateTime(dt), zone, format);
        }

        /// <summary>
        /// UTC -> 국가별 시간으로 변환하기
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="zone"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime ConvertTimeFromUtc(this string dt, string zone = "Korea Standard Time")
        {
            return ConvertTimeFromUtcByDateTime(Convert.ToDateTime(dt), zone);
        }

        /// <summary>
        /// DateTime을 TimeStamp으로 변환하기
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static double ConvertToUnixTimestamp(DateTime date)
        {
            var timestemp = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timestemp.TotalSeconds;
        }

        /// <summary>
        /// TimeStamp를 DateTime으로 변환하기
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        /// <summary>
        /// 우선 US지사만 사용하므로 하드코딩 진행
        /// </summary>
        /// <remarks>
        /// US의 경우는 2곳이 존재하지만 CA기준으로 가지고 오도록 한다.(관리팀요청사항)
        /// https://greenwichmeantime.com/time-zone/gmt-8/ 사이트에서 확인하면됨
        /// </remarks>
        public static Dictionary<string, int> GetGMTTIme = new Dictionary<string, int>()
        {
            { "CO000007", -8  }, //CA
            { "CO000001", 9  } //KOR
        };
    }
}
