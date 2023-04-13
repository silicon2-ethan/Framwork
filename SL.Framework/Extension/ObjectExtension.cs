using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SL.Framework.Extension
{
    /// <summary>
    /// OBJECT EXTENSION CLASS
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// OBJECT 를 T 타입으로 형변환.
        /// OBJECT 가 NULL 이거나 실패 시 예외.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ConvertType<T>(this object value)
        {
            if (IsNullObject(value))
                throw new Exception("형변환(ConvertType<T>) 대상이 NULL 입니다.");

            try
            {
                return (T)value;
            }
            catch
            {
                throw new Exception("형변환(ConvertType<T>) 중 오류가 발생하였습니다.");
            }
        }

        /// <summary>
        /// OBJECT 를 T 타입으로 형변환.
        /// OBJECT 가 NULL 이거나 형변환 실패 시 기본값 Def 반환.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T ConvertType<T>(this object value, T def)
        {
            if (IsNullObject(value))
                return def;

            try
            {
                return (T)value;
            }
            catch (Exception)
            {
                return def;
            }
        }

        /// <summary>
        /// OBJECT 의 NULL 체크
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullObject(object value)
        {
            return value == null || value == DBNull.Value;
        }

        /// <summary>
        /// OBJECT 를 STRING 형식으로 형변환.
        /// OBJECT 가 NULL 이거나 형변환 실패 시 기본값 Def 반환.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def">기본값</param>
        /// <returns></returns>
        public static string ToString(this object value, string def)
        {
            return value.ConvertType(def);
        }

        /// <summary>
        /// OBJECT 를 DATETIME 형식으로 형변환.
        /// OBJECT 가 NULL 이거나 실패 시 예외.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object value)
        {
            return ConvertType<DateTime>(value);
        }

        /// <summary>
        /// OBJECT 를 DATETIME 형식으로 형변환.
        /// OBJECT 가 NULL 이거나 형변환 실패 시 기본값 Def 반환.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object value, DateTime def)
        {
            return value.ConvertType(def);
        }

        /// <summary>
        /// OBJECT 를 INT16 형식으로 형변환.
        /// OBJECT 가 NULL 이거나 실패 시 예외.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int16 ToInt16(this object value)
        {
            return ConvertType<Int16>(value);
        }

        /// <summary>
        /// OBJECT 를 INT16 형식으로 형변환.
        /// OBJECT 가 NULL 이거나 형변환 실패 시 기본값 Def 반환.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static Int16 ToInt16(this object value, Int16 def)
        {
            return value.ConvertType(def);
        }

        /// <summary>
        /// OBJECT 를 INT32 형식으로 형변환.
        /// OBJECT 가 NULL 이거나 실패 시 예외.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int32 ToInt32(this object value)
        {
            return ConvertType<Int32>(value);
        }

        /// <summary>
        /// OBJECT 를 INT32 형식으로 형변환.
        /// OBJECT 가 NULL 이거나 형변환 실패 시 기본값 Def 반환.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static Int32 ToInt32(this object value, Int32 def)
        {
            return value.ConvertType(def);
        }

        /// <summary>
        /// OBJECT 를 INT64 형식으로 형변환.
        /// OBJECT 가 NULL 이거나 실패 시 예외.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int64 ToInt64(this object value)
        {
            return ConvertType<Int64>(value);
        }

        /// <summary>
        /// OBJECT 를 INT64 형식으로 형변환.
        /// OBJECT 가 NULL 이거나 형변환 실패 시 기본값 Def 반환.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static Int64 ToInt64(this object value, Int64 def)
        {
            return value.ConvertType(def);
        }

        /// <summary>
        /// OBJECT 를 DECIMAL 형식으로 형변환.
        /// OBJECT 가 NULL 이거나 실패 시 예외.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Decimal ToDecimal(this object value)
        {
            return ConvertType<Decimal>(value);
        }

        /// <summary>
        /// OBJECT 를 DECIMAL 형식으로 형변환.
        /// OBJECT 가 NULL 이거나 형변환 실패 시 기본값 Def 반환.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static Decimal ToDecimal(this object value, Decimal def)
        {
            return value.ConvertType(def);
        }

        /// <summary>
        /// OBJECT 를 DECIMAL 형식으로 형변환.
        /// OBJECT 가 NULL 이거나 실패 시 예외.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static short ToShort(this object value)
        {
            return ConvertType<short>(value);
        }

        /// <summary>
        /// OBJECT 를 DECIMAL 형식으로 형변환.
        /// OBJECT 가 NULL 이거나 형변환 실패 시 기본값 Def 반환.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static short ToShort(this object value, short def)
        {
            return value.ConvertType(def);
        }

        /// <summary>
        /// OBJECT 를 BYTE 형식으로 형변환.
        /// OBJECT 가 NULL 이거나 실패 시 예외.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ToByte(this object value)
        {
            return ConvertType<byte>(value);
        }

        /// <summary>
        /// OBJECT 를 BYTE 형식으로 형변환.
        /// OBJECT 가 NULL 이거나 형변환 실패 시 기본값 Def 반환.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static byte ToByte(this object value, byte def)
        {
            return value.ConvertType(def);
        }

        /// <summary>
        /// OBJECT 를 SBYTE 형식으로 형변환.
        /// OBJECT 가 NULL 이거나 실패 시 예외.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static sbyte ToSByte(this object value)
        {
            return ConvertType<sbyte>(value);
        }

        /// <summary>
        /// OBJECT 를 SBYTE 형식으로 형변환.
        /// OBJECT 가 NULL 이거나 형변환 실패 시 기본값 Def 반환.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static sbyte ToSByte(this object value, sbyte def)
        {
            return value.ConvertType(def);
        }

        /// <summary>
        /// DataRow의 Column 값 반환
        /// </summary>
        /// <param name="row">DataRow</param>
        /// <param name="columnName">컬럼명</param>
        /// <returns></returns>
        public static object GetColumnValue(this DataRow row, string columnName)
        {
            return row == null ? "" : row[columnName];
        }

        /// <summary>
        /// DataRow의 Column 값 반환
        /// </summary>
        /// <param name="row">DataRow</param>
        /// <param name="columnName">컬럼명</param>
        /// <param name="def">기본값</param>
        /// <returns></returns>
        public static object GetColumnValue(this DataRow row, string columnName, object def)
        {
            try
            {
                return row == null ? def : row[columnName];
            }
            catch
            {
                return def;
            }
        }

        /// <summary>
        /// DataRow를 Dictionary형으로 변환하여 반환한다.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(this DataRow source)
        {
            return source == null
                ? null
                : source.Table.Columns.Cast<DataColumn>()
                    .ToDictionary(column => column.ColumnName, column => source[column]);
        }

        /// <summary>
        /// List<Dictionary/> 특정 키값 검색
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value">value</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetDictionary(this List<Dictionary<string, object>> source,
            object value)
        {
            return (from n in source.AsEnumerable()
                where n["value"].ToString().Equals(value.ToString(), StringComparison.OrdinalIgnoreCase)
                select n).ToArray()[0];
        }

        /// <summary>
        /// 모델을 JSON String으로 변환
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj, Formatting format = Formatting.None)
        {
            return JsonConvert.SerializeObject(obj, format);
        }
    }
}