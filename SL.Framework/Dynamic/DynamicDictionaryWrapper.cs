using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.Framework.Dynamic
{
    /// <summary>
    /// dynamic 객체를 dictionary 객체로 재 정의
    /// </summary>
    /// <example>
    /// dynamic person = new DynamicDictionary();
    /// person.FirstName = "Ellen";
    /// person.LastName = "Adams";
    /// Console.WriteLine(person.firstname + " " + person.lastname);
    /// Console.WriteLine("Number of dynamic properties:" + person.Count);    
    /// </example>
    public sealed class DynamicDictionaryWrapper : DynamicObject
    {
        #region [Properties]
        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, object> dictionary { get; set; }

        public Dictionary<string, object> Element { get { return dictionary; } }

        /// <summary>
        /// 
        /// </summary>
        public int Count { get { return dictionary.Count; } }
        #endregion

        /// <summary>
        /// 생성자
        /// </summary>
        public DynamicDictionaryWrapper()
            : base()
        {
            if (dictionary == null)
                dictionary = new Dictionary<string, object>();
        }

        #region [Members]
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string name = binder.Name.ToLower();
            return dictionary.TryGetValue(name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (dictionary.ContainsKey(binder.Name) == false)
                dictionary[binder.Name.ToLower()] = value;
            return true;
        }

        //더 필요한 경우 override 형태로 추가 메소드를 넣는다.

        #endregion

        /// <summary>
        ///  FirstOrDefault
        /// </summary>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public object FirstOrDefault(string elementName)
        {
            return dictionary.FirstOrDefault(x => x.Key.ToString().Equals(elementName));
        }

        /// <summary>
        ///  Where
        /// </summary>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, object>> Where(string elementName)
        {
            return dictionary.Where(x => x.Key.ToString().Equals(elementName));
        }

        /// <summary>
        /// 실체화된 dynamic 객체 리턴
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> ConvertDynamicToDictonary()
        {
            return dictionary;
        }


    }
}
