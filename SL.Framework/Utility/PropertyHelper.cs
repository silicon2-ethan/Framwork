using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SL.Framework.Utility
{
    public static class PropertyHelper
    {
        /// <summary>
        /// Null 허용 타입의 값을 원하는 값으로 초기화
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="typeConfigs"></param>
        /// <returns></returns>
        public static List<T> InitNullValue<T>(List<T> list, params KeyValuePair<Type, object>[] typeConfigs)
        {
            if (list == null) { return null; }

            List<T> copyList = new List<T>(list);

            foreach (var item in copyList)
            {
                foreach (var p in item.GetType().GetProperties())
                {
                    foreach (KeyValuePair<Type, object> config in typeConfigs)
                    {
                        if (p.PropertyType == config.Key && p.PropertyType.IsValueType == false)
                        {
                            if (p.GetValue(item, null) == null)
                            {
                                p.SetValue(item, config.Value, null);
                            }
                        }
                    }
                }
            }

            return copyList;
        }
    }
}
