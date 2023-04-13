using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SL.Common
{
    #region [BasicEntity<T>]
    public class BasicEntity<T> where T : class, new()
    {
        static readonly object lockObject = new object();
        static T instance { get; set; }

        public static T Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }
                return instance;

            }
        }
    }
    #endregion
    
    #region [DaoWrapper Entity]
    /// <summary>
    /// reflection invoke 호출용
    /// </summary>
    public class DaoWrapper
    {
        public string id { get; set; }
        public string declaretype { get; set; }
        public string methodname { get; set; }
        public string assembly { get; set; }
        public string Invoke { get; set; }
    }
    #endregion
}
