using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SL.Framework.Utility
{
    /// <summary>
    /// 체크하는 목적으로 사용하는 클래스 
    /// 정규식 체크도 상황봐서 이쪽으로 넣어준다. 
    /// </summary>
    public static class ValidatorHelper
    {
        #region [Required]

        /// <summary>
        /// 값을 체크할 때 사용한다.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool vRequired(this int value)
        {
            return value > 0;
        }

        /// <summary>
        /// 값을 체크할 때 사용한다.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool vRequired(this int? value)
        {
            if (value == null)
                value = 0;
            return !(value <= 0);
        }

        /// <summary>
        /// 값을 체크할 때 사용한다.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool vRequired(this long value)
        {
            return value > 0;
        }

        /// <summary>
        /// 값을 체크할 때 사용한다.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool vRequired(this long? value)
        {
            if (value == null)
                value = 0;
            return !(value <= 0);
        }

        /// <summary>
        /// 값을 체크할 때 사용한다.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool vRequired(this float value)
        {
            return !(value <= 0);
        }

        /// <summary>
        /// 값을 체크할 때 사용한다.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool vRequired(this float? value)
        {
            if (value == null)
                value = 0;
            return !(value <= 0);
        }

        /// <summary>
        /// 값을 체크할 때 사용한다.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool vRequired(this double value)
        {
            return !(value <= 0);
        }

        /// <summary>
        /// 값을 체크할 때 사용한다.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool vRequired(this double? value)
        {
            if (value == null)
                value = 0;
            return !(value <= 0);
        }

        /// <summary>
        /// 값을 체크할 때 사용한다.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool vRequired(this decimal value)
        {
            return value > 0;
        }

        /// <summary>
        /// 값을 체크할 때 사용한다.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool vRequired(this decimal? value)
        {
            if (value == null)
                value = 0;
            return !(value <= 0);
        }

        /// <summary>
        /// 값을 체크할 때 사용한다.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool vRequired(this string value)
        {
            if (string.IsNullOrEmpty(value))
                value = "";
            return value != "";
        }

        #endregion

        #region [DefaultValue]

        /// <summary>
        /// 기본값 세팅 추후 변수를 생성하고, 까먹고 체크안할 때 사용
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T vDefaultValue<T>(this T t)
        {
            if (t != null) return t;
            object r = default(T);
            try
            {
                if (r == null) return (T)((object)"");
            }
            catch (Exception)
            {
            }
            return default(T);
        }

        /// <summary>
        /// 기본값 세팅 추후 변수를 생성하고, 까먹고 체크안할 때 사용
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static int? vDefaultValue(this int? t)
        {
            if (t != null) return t;
            return t.GetValueOrDefault();
        }

        /// <summary>
        /// 기본값 세팅 추후 변수를 생성하고, 까먹고 체크안할 때 사용
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static long? vDefaultValue(this long? t)
        {
            if (t != null) return t;
            return t.GetValueOrDefault();
        }

        /// <summary>
        /// 기본값 세팅 추후 변수를 생성하고, 까먹고 체크안할 때 사용
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float? vDefaultValue(this float? t)
        {
            if (t != null) return t;
            return t.GetValueOrDefault();
        }

        /// <summary>
        /// 기본값 세팅 추후 변수를 생성하고, 까먹고 체크안할 때 사용
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static double? vDefaultValue(this double? t)
        {
            if (t != null) return t;
            return t.GetValueOrDefault();
        }

        /// <summary>
        /// 기본값 세팅 추후 변수를 생성하고, 까먹고 체크안할 때 사용
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T vDefaultValue<T>(this T t, object value)
        {
            if (value == null) return default(T);

            if (value.GetType() == typeof(string))
            {
                if (string.IsNullOrEmpty(value.ToString()))
                {
                    value = "";
                }
                var tmp = t.vDefaultValue();
                if (string.IsNullOrEmpty(tmp.ToString()))
                {
                    return (T)((object)value);
                }
                else
                {
                    return tmp;
                }
            }
            return (T)((object)value);
        }

        /// <summary>
        /// 기본값 세팅 추후 변수를 생성하고, 까먹고 체크안할 때 사용
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static DateTime vDefaultValue(this DateTime t)
        {
            return t == DateTime.MinValue ? Convert.ToDateTime("1900-01-01 00:00:00") : t;
        }

        /// <summary>
        /// 서비스 국가정보를 넘겨준다.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Service_Nation_Code(this string value)
        {
            var svc_nation_cd = value.vDefaultValue();
            if (svc_nation_cd == "") return "SG";
            if (svc_nation_cd == "JP") return "ebay";
            return svc_nation_cd;
        }

        /// <summary>
        /// NULL 포함 문자열 빈값 체크
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool vIsEmpty(this string value)
        {
            return value.vDefaultValue() == "";
        }

        /// <summary>
        /// 빈문자열 아님 체크
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool vIsNotEmpty(this string value)
        {
            return value.vDefaultValue() != "";
        }

        // Only useful before .NET 4
        public static void CopyTo(this Stream input, Stream output)
        {
            byte[] buffer = new byte[16 * 1024]; // Fairly arbitrary size
            int bytesRead;

            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }

        /// <summary>
        /// string을 원하는 형으로 convert하는 메소드
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <example>
        ///  string test4 = "2018-13-01 00:00:00";
        ///  Console.WriteLine(test4.vToConvert<long>());
        /// </example>
        public static T vToConvert<T>(this string value)
        {
            Type undertype = Nullable.GetUnderlyingType(typeof(T)); // Test for Nullable<T> and return the base type instead:
            Type basetype = (undertype == null) ? typeof(T) : undertype;
            string temp = value.vDefaultValue();

            #region [CheckValidateString]
            Func<string, string> CheckValidateString = delegate (string checkValue)
            {
                if (
                    (basetype == typeof(sbyte))
                    || (basetype == typeof(byte))
                    || (basetype == typeof(ushort))
                    || (basetype == typeof(uint))
                    || (basetype == typeof(ulong))
                    || (basetype == typeof(short))
                    || (basetype == typeof(int))
                    || (basetype == typeof(long)))
                {
                    checkValue = "0";
                }
                else if (
                    (basetype == typeof(float))
                    || (basetype == typeof(double))
                    || (basetype == typeof(decimal)))
                {
                    checkValue = "0.0";
                }
                else if (basetype == typeof(DateTime))
                {
                    checkValue = "1900-01-01 00:00:00";
                }
                return checkValue;
            };
            #endregion

            try
            {
                if (temp.vIsEmpty())
                    temp = CheckValidateString(temp);
                return (T)Convert.ChangeType(temp, basetype);
            }
            catch (System.FormatException)
            {
                temp = CheckValidateString(temp);
                return (T)Convert.ChangeType(temp, basetype);
            }
            catch
            {
                //기타 에러 메시지일 경우 처리
                temp = CheckValidateString(temp);
                return (T)Convert.ChangeType(temp, basetype);
            }
        }

        /// <summary>
        /// 배열에 존재하는 값을 문자열로 치환
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separatedValue"></param>
        /// <returns></returns>
        /// <example>
        /// List<string> test = new List<string>();
        /// test.Add("1");
        /// test.Add("2");
        /// test.Add("3");
        /// test.Add("4");
        /// test.Add("5");
        /// Console.WriteLine(test.vJoin(","));
        /// </example>
        public static string vJoin(this string[] value, string separatedValue = ",")
        {
            string returnValue = "";

            if (value.Length > 0)
                returnValue = string.Join(separatedValue, value);

            return returnValue;
        }

        /// <summary>
        /// 배열에 존재하는 값을 문자열로 치환
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separatedValue"></param>
        /// <returns></returns>
        /// <example>
        /// string[] test1 = new string[] { "1", "2", "3", "4", "5" };
        /// Console.WriteLine(test1.vJoin(","));
        /// </example>
        public static string vJoin(this List<string> value, string separatedValue = ",")
        {
            if (value == null)
                value = new List<string>();

            return value.ToArray().vJoin(separatedValue);
        }

        /// <summary>
        /// 배열에 존재하는 값을 문자열로 치환
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separatedValue"></param>
        /// <returns></returns>
        /// <example>
        /// string[] test1 = new string[] { "1", "2", "3", "4", "5" };
        /// Console.WriteLine(test1.vJoin(","));
        /// </example>
        public static string vJoin(this List<object> value, string separatedValue = ",")
        {
            if (value == null)
                value = new List<object>();

            List<string> convertLists = new List<string>();
            if (value.First().GetType() == typeof(string))
            {
                foreach (object item in value)
                {
                    convertLists.Add(item.ToString());
                }
            }
            return convertLists.ToArray().vJoin(separatedValue);
        }

        /// <summary>
        /// 배열에 존재하는 값을 문자열로 치환
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnName"></param>
        /// <param name="separatedValue"></param>
        /// <returns></returns>
        /// <example>
        /// DataTable dt = new DataTable();
        /// dt.Columns.Add("test", typeof(string));
        /// for (int i = 0; i < 10; i++)
        /// {
        ///     DataRow dr = dt.NewRow();
        ///     dr["test"] = i.ToString();
        ///     dt.Rows.Add(dr);                
        /// }
        /// Console.WriteLine(dt.vJoin("test", "!"));        
        /// </example>
        public static string vJoin(this DataTable dt, string columnName, string separatedValue = ",")
        {
            if (dt == null || dt.Rows.Count <= 0) return "";
            if (dt.Columns.Contains(columnName) == false) return "";

            var data = dt.AsEnumerable().Select(x => x[columnName]).ToList();
            return data.vJoin(separatedValue);
        }

        /// <summary>
        /// DataRow Object -> Int32 변환
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Int32 GetInt32(this DataRow row, string columnName, Int32 defaultValue = 0)
        {
            Int32 convertedValue = defaultValue;

            try
            {
                convertedValue = Convert.ToInt32(row[columnName]);
            }
            catch {}

            return convertedValue;
        }

        /// <summary>
        /// DataRow Object -> Int64 변환
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Int64 GetInt64(this DataRow row, string columnName, Int64 defaultValue = 0)
        {
            Int64 convertedValue = defaultValue;

            try
            {
                convertedValue = Convert.ToInt64(row[columnName]);
            }
            catch {}

            return convertedValue;
        }

        public static string GetString(this DataRow row, string columnName, string defaultValue = "")
        {
            string convertedValue = defaultValue;

            try
            {
                if (row[columnName] != DBNull.Value)
                {
                    convertedValue = Convert.ToString(row[columnName]).Trim();
                }
            }
            catch { }

            return convertedValue;
        }

        public static decimal GetDecimal(this DataRow row, string columnName, decimal defaultValue = 0.0M)
        {
            decimal convertedValue = defaultValue;

            try
            {
                if (row[columnName] != DBNull.Value)
                {
                    convertedValue = Convert.ToDecimal(row[columnName]);
                }
            }
            catch { }


            return convertedValue;
        }

        #endregion

        #region [ETC]
        public static bool vDictionariesEqual<K, V>(Dictionary<K, V> d1, Dictionary<K, V> d2)
        {
            return d1.Keys.Count == d2.Keys.Count
              && d1.Keys.All(k => d2.ContainsKey(k) && object.Equals(d1[k], d2[k]));
        }

        public static T[] vToArray<T>(params T[] p)
        {
            return p;
        }

        public static string vToCamelCase(this string s)
        {
            if (s.Length > 1)
            {
                return s.Substring(0, 1).ToLower() + s.Substring(1);
            }
            else if (s.Length == 1)
            {
                return s.Substring(0, 1).ToLower();
            }
            else
            {
                return s;
            }
        }

        public static string vTypeToSerializationName(this Type type, String suffix)
        {
            var typeName = type.Name;
            if (typeName == suffix) return "none";
            var name = (typeName.EndsWith(suffix)) ? typeName.Substring(0, typeName.Length - suffix.Length) : typeName;
            name = name.vToCamelCase();
            return name;
        }
        #endregion
    }
}