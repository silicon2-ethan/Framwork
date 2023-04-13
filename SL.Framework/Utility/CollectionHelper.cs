using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SL.Framework.Utility
{
    /// <summary>
    /// Extend methods of the datatable object  to Convert to generic.
    /// </summary>
    public static class DatatableHelper
    {
        /// <summary>
        /// Convert datatable to Generic List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> vMapperList<T>(this DataTable dt) where T : new()
        {
            var dataList = new List<T>();

            //Define what attributes to be read from the class
            const System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;

            //Read Attribute Names and Types
            var objFieldNames = typeof(T).GetProperties(flags).Cast<System.Reflection.PropertyInfo>().Select(item => new
            {
                Name = item.Name,
                Type = Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType
            }).ToList();

            //Read Datatable column names and types
            var dtlFieldNames = dt.Columns.Cast<DataColumn>().Select(item => new
            {
                Name = item.ColumnName,
                Type = item.DataType
            }).ToList();

            foreach (DataRow dataRow in dt.AsEnumerable().ToList())
            {
                var classObj = new T();
                foreach (var dtField in dtlFieldNames)
                {
                    System.Reflection.PropertyInfo propertyInfos = classObj.GetType().GetProperty(dtField.Name);
                    var field = objFieldNames.Find(x => x.Name == dtField.Name);
                    if (field != null)
                    {
                        if (propertyInfos.PropertyType == typeof(DateTime))
                        {
                            propertyInfos.SetValue(classObj, convertToDateTime(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(Nullable<DateTime>))
                        {
                            propertyInfos.SetValue(classObj, convertToDateTime(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(int))
                        {
                            propertyInfos.SetValue(classObj, ConvertToInt(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(long))
                        {
                            propertyInfos.SetValue(classObj, ConvertToLong(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(decimal))
                        {
                            propertyInfos.SetValue(classObj, ConvertToDecimal(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(double))
                        {
                            propertyInfos.SetValue(classObj, ConvertToDouble(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(String))
                        {
                            if (dataRow[dtField.Name].GetType() == typeof(DateTime))
                            {
                                propertyInfos.SetValue(classObj, ConvertToDateString(dataRow[dtField.Name]), null);
                            }
                            else
                            {
                                propertyInfos.SetValue(classObj, ConvertToString(dataRow[dtField.Name]), null);
                            }
                        }
                        else
                        {
                            Type t = Nullable.GetUnderlyingType(propertyInfos.PropertyType) ?? propertyInfos.PropertyType;
                            t = Nullable.GetUnderlyingType(t) ?? t;
                            object safeValue = (dataRow[dtField.Name] == null || DBNull.Value.Equals(dataRow[dtField.Name])) ? default(T) : (T)Convert.ChangeType(dataRow[dtField.Name], t);
                            propertyInfos.SetValue(classObj, safeValue, null);
                        }
                    }
                }
                dataList.Add(classObj);
            }
            return dataList;
        }

        /// <summary>
        /// Convert datatable to a Generic object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T vMapperObject<T>(this DataTable dt) where T : new()
        {
            // Create a new type of the entity I want  
            Type t = typeof(T);
            T returnObject = new T();

            string colName = "";
            foreach (DataColumn col in dt.Columns)
            {
                colName = col.ColumnName;

                foreach (DataRow dr in dt.Rows)
                {
                    // Look for the object's property with the columns name, ignore case  
                    PropertyInfo pInfo = t.GetProperty(colName.ToLower(),
                                                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    // did we find the property ?  
                    if (pInfo != null)
                    {
                        object val = dr[colName];

                        // is this a Nullable<> type  
                        bool IsNullable = (Nullable.GetUnderlyingType(pInfo.PropertyType) != null);
                        if (IsNullable)
                        {
                            if (val is System.DBNull)
                            {
                                val = null;
                            }
                            else
                            {
                                // Convert the db type into the T we have in our Nullable<T> type  
                                val = Convert.ChangeType(val, Nullable.GetUnderlyingType(pInfo.PropertyType));
                            }
                        }
                        else
                        {
                            // Convert the db type into the type of the property in our entity  
                            if (val is System.DBNull)
                                val = null;
                            else
                                val = Convert.ChangeType(val, pInfo.PropertyType);
                        }
                        // Set the value of the property with the value from the db  
                        pInfo.SetValue(returnObject, val, null);
                    }
                } //foreach (DataRow dr in dt.Rows)

            } //foreach (DataColumn col in dt.Columns)
            return returnObject;
        }

        /// <summary>
        /// convert to hashtable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<Hashtable> vMapperToHashtable(this DataTable dt)
        {
            List<Hashtable> returnValue = new List<Hashtable>();

            foreach (DataRow dataRow in dt.Rows)
            {
                Hashtable ht = new Hashtable();
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    ht.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName]);
                } //dc
                returnValue.Add(ht);
            } //foreach (DataRow dataRow in dt.Rows)
            return returnValue;
        }


        /// <summary>
        /// convert to one of hashtable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Hashtable vMapperToOneHashtable(this DataTable dt)
        {
            Hashtable ht = new Hashtable();

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    ht.Add(dataColumn.ColumnName, dr[dataColumn.ColumnName]);
                }
            }
            return ht;
        }


        /// <summary>
        /// 화면에 노출할 JSON형태의 데이터를 제공한다.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static dynamic ToJSON(this DataTable data)
        {
            #region [Result]
            List<Dictionary<string, string>> resultData = new List<Dictionary<string, string>>();
            foreach (DataRow item in data.Rows)
            {
                Dictionary<string, string> rowData = new Dictionary<string, string>();
                foreach (DataColumn dc in data.Columns)
                {
                    rowData.Add(dc.ColumnName, item[dc.ColumnName].ToString().vRegExClearScriptTag());
                }
                resultData.Add(rowData);
            }
            #endregion
            return resultData;
        }


        #region [Private method]
        private static string ConvertToDateString(object date)
        {
            if (date == null)
                return string.Empty;

            return date == null ? string.Empty : Convert.ToDateTime(date).ConvertDate();
        }

        private static string ConvertToString(object value)
        {
            return Convert.ToString(ReturnEmptyIfNull(value));
        }

        private static int ConvertToInt(object value)
        {
            return Convert.ToInt32(ReturnZeroIfNull(value));
        }

        private static long ConvertToLong(object value)
        {
            return Convert.ToInt64(ReturnZeroIfNull(value));
        }

        private static decimal ConvertToDecimal(object value)
        {
            return Convert.ToDecimal(ReturnZeroIfNull(value));
        }

        private static double ConvertToDouble(object value)
        {
            return Convert.ToDouble(ReturnZeroIfNull(value));
        }

        private static DateTime convertToDateTime(object date)
        {
            return Convert.ToDateTime(ReturnDateTimeMinIfNull(date));
        }

        public static string ConvertDate(this DateTime datetTime, bool excludeHoursAndMinutes = false)
        {
            if (datetTime != DateTime.MinValue)
            {
                if (excludeHoursAndMinutes)
                    return datetTime.ToString("yyyy-MM-dd");
                return datetTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            return null;
        }
        public static object ReturnEmptyIfNull(this object value)
        {
            if (value == DBNull.Value)
                return string.Empty;
            if (value == null)
                return string.Empty;
            return value;
        }
        public static object ReturnZeroIfNull(this object value)
        {
            if (value == DBNull.Value)
                return 0;
            if (value == null)
                return 0;
            return value;
        }
        public static object ReturnDateTimeMinIfNull(this object value)
        {
            if (value == DBNull.Value)
                return DateTime.MinValue;
            if (value == null)
                return DateTime.MinValue;
            return value;
        }
        #endregion
    }

    /// <summary>
    ///  System Collections
    /// </summary>
    public static class CollectionHelper
    {
        /// <summary>
        /// class -> Dictionary 변경
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <remarks>
        /// TestClass testClass = new TestClass() { Test1 = "1", Test2 = "2", Test3 = 1 };
        /// vMapperToDictionary<TestClass>(testClass)
        /// </remarks>
        public static Dictionary<string, string> vMapperToDictionary<T>(T t) where T : class
        {
            Dictionary<string, string> returnValue = new Dictionary<string, string>();
            Type typeObj = t.GetType();
            PropertyInfo[] propInfos = typeObj.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propInfo in propInfos)
            {
                if (propInfo.GetValue(t, null) != null)
                    returnValue.Add(propInfo.Name, propInfo.GetValue(t, null).ToString());
                else
                {
                    returnValue.Add(propInfo.Name, null);
                }
            }
            return returnValue;
        }

        public static string GetXmlParamSimple<T>(T t, bool isRoot = true) where T: class
        {
            return vMapperToDictionary<T>(t).GetXmlInputParameters(isRoot);
        }

        /// <summary>
        /// List  -> List Dictionary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <remarks>
        /// List<TestClass> testClass1 = new List<TestClass>() { new TestClass() { Test1 = "1", Test2 = "2", Test3 = 1 } };
        /// vMapperToListDictory<List<TestClass>>(testClass1);
        /// </remarks>
        public static List<Dictionary<string, string>> vMapperToListDictionary<T>(T t) where T : IList
        {
            List<Dictionary<string, string>> returnValue = new List<Dictionary<string, string>>();
            if (t == null) return returnValue;
            PropertyInfo[] properties = t.GetType().GetProperties();
            foreach (var item in t)
            {
                Dictionary<string, string> entity = new Dictionary<string, string>();
                foreach (PropertyInfo property in item.GetType().GetProperties())
                {
                    if (property.GetValue(item, null) != null)
                        entity.Add(property.Name, property.GetValue(item, null).ToString());
                    else
                    {
                        entity.Add(property.Name, null);
                    }
                } //foreach (PropertyInfo property in item.GetType().GetProperties())
                returnValue.Add(entity);
            } //foreach (var item in t)
            return returnValue;
        }

        public static string GetXmlListParamSimple<T>(T t, bool isRoot = true) where T : IList
        {
            return vMapperToListDictionary<T>(t).GetXmlInputParameters(isRoot);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T vMapperToObject<T>(this Hashtable source) where T : new()
        {
            Type t = typeof(T);
            T returnObject = new T();

            foreach (string item in source.Keys)
            {
                PropertyInfo pInfo = t.GetProperty(item.ToLower(),
                                                          BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                // did we find the property ?  
                if (pInfo != null)
                {
                    object val = source[item];

                    // is this a Nullable<> type  
                    bool IsNullable = (Nullable.GetUnderlyingType(pInfo.PropertyType) != null);
                    if (IsNullable)
                    {
                        if (val is System.DBNull)
                        {
                            val = null;
                        }
                        else
                        {
                            // Convert the db type into the T we have in our Nullable<T> type  
                            val = Convert.ChangeType(val, Nullable.GetUnderlyingType(pInfo.PropertyType));
                        }
                    }
                    else
                    {
                        // Convert the db type into the type of the property in our entity  
                        if (val is System.DBNull)
                            val = null;
                        else
                            val = Convert.ChangeType(val, pInfo.PropertyType);
                    }
                    // Set the value of the property with the value from the db  
                    pInfo.SetValue(returnObject, val, null);
                }
            } //foreach (string item in source.Keys)

            return returnObject;
        }

        /// <summary>
        ///  copy class to class
        /// </summary>
        /// <typeparam name="T">source</typeparam>
        /// <typeparam name="U">target</typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static U vMapperToMapper<T, U>(T t) where T : class where U : new()
        {
            Type u = typeof(U);
            U returnObject = new U();

            Type typeObj = t.GetType();
            PropertyInfo[] propInfos = typeObj.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propInfo in propInfos)
            {
                PropertyInfo pInfo = u.GetProperty(propInfo.Name.ToLower(),
                                              BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (pInfo != null)
                {
                    object val = propInfo.GetValue(t, null);
                    // is this a Nullable<> type  
                    bool IsNullable = (Nullable.GetUnderlyingType(pInfo.PropertyType) != null);
                    if (IsNullable)
                    {
                        if (val is System.DBNull)
                        {
                            val = null;
                        }
                        else
                        {
                            // Convert the db type into the T we have in our Nullable<T> type  
                            val = Convert.ChangeType(val, Nullable.GetUnderlyingType(pInfo.PropertyType));
                        }
                    }
                    else
                    {
                        // Convert the db type into the type of the property in our entity  
                        try
                        {
                            if (val is System.DBNull)
                                val = null;
                            else if (val is null)
                                val = null;
                            else
                                val = Convert.ChangeType(val, pInfo.PropertyType);
                        }
                        catch (Exception)
                        {
                            val = null;
                        }
                    }
                    // Set the value of the property with the value from the db  
                    try
                    {
                        pInfo.SetValue(returnObject, val, null);
                    }
                    catch (Exception)
                    {
                        pInfo.SetValue(returnObject, null, null); //format 오류가 발생할 수 있어서 null로 넘김
                    }
                }
            }
            return returnObject;
        }

        /// <summary>
        /// copy data of class to data of class(데이터까지 복사 new 개념이 아님)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="t"></param>
        /// <param name="u"></param>
        /// <returns></returns>
        public static U vMapperDataToMapperData<T, U>(T t, U u) where T : class where U : class
        {
            Type typeObj = t.GetType();
            Type typeObj1 = u.GetType();


            PropertyInfo[] propInfos = typeObj.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propInfo in propInfos)
            {
                PropertyInfo pInfo = typeObj1.GetProperty(propInfo.Name.ToLower(),
                                              BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (pInfo != null)
                {
                    object val = propInfo.GetValue(t, null);
                    // is this a Nullable<> type  
                    bool IsNullable = (Nullable.GetUnderlyingType(pInfo.PropertyType) != null);
                    if (IsNullable)
                    {
                        if (val is System.DBNull)
                        {
                            val = null;
                        }
                        else
                        {
                            // Convert the db type into the T we have in our Nullable<T> type  
                            val = Convert.ChangeType(val, Nullable.GetUnderlyingType(pInfo.PropertyType));
                        }
                    }
                    else
                    {
                        // Convert the db type into the type of the property in our entity  
                        if (val is System.DBNull)
                            val = null;
                        else
                            val = Convert.ChangeType(val, pInfo.PropertyType);
                    }
                    // Set the value of the property with the value from the db  
                    try
                    {
                        pInfo.SetValue(u, val, null);
                    }
                    catch (Exception)
                    {
                        pInfo.SetValue(u, null, null);
                    }

                }
            }
            return u;
        }
    }

}
