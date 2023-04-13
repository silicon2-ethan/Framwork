using Newtonsoft.Json;
using SL.Framework.Dynamic;
using SL.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SL.Framework.Extension
{
    /// <summary>
    /// MULTILAN EXTENSION CLASS
    /// </summary>
    public class MultiLangExtension
    {
        static string[] KeyOfResource = new string[] { "en", "ko-KR", "ja-JP", "zh-CN", "ru-RU", "es-ES" };

        /// <summary>
        /// LOCK 객체
        /// </summary>
        private static readonly Object ObjLock = new object();

        /// <summary>
        /// 현재 선택된 국가의 리소스를 가지고 온다.
        /// </summary>
        //static List<Tuple<string, string>> currentResource { get; set; }

        /// <summary>
        /// 정적으로 사용할 리소스(Biz에서 사용함)
        /// </summary>
        public static Dictionary<string, List<Tuple<string, string>>> Resources { get; private set; }

        /// <summary>
        /// 정적으로 사용할 리소스(Biz에서 사용함)
        /// </summary>
        public static List<DynamicDictionaryWrapper> ResourceData { get; private set; }

        /// <summary>
        /// GET LANGUAGE COOKIE VALUE
        /// </summary>
        public static string GetLanguage { get; set; }

        /// <summary>
        /// 다국어 지원을 위한 초기화
        /// </summary>
        /// <param name="path"></param>
        public static void Init()
        {
            // 초기화
            if (Resources == null)
                Resources = new Dictionary<string, List<Tuple<string, string>>>();
            else
            {
                lock (ObjLock)
                    Resources.Clear();
            }

            if (ResourceData == null)
                ResourceData = new List<DynamicDictionaryWrapper>();
        }

        /// <summary>
        /// DB 에서 리소스 데이터를 읽어서 저장한다.
        /// </summary>
        /// <param name="data"></param>
        public static void AddResources(DynamicDictionaryWrapper data)
        {
            lock (ObjLock)
            {
                ResourceData.Add(data);
            }
        }

        /// <summary>
        /// DB 에서 리소스 데이터를 읽어서 저장한다.
        /// </summary>
        /// <param name="data"></param>
        public static void ClearResources()
        {
            lock (ObjLock)
            {
                ResourceData.Clear();
            }
        }

        /// <summary>
        /// 리소스를 가지고 온다.
        /// </summary>
        public static void GetResources()
        {
            if (ResourceData.Count <= 0) return;

            #region [국가별 그룹을 호출한다.]
            foreach (var item in KeyOfResource)
            {
                Resources.Add(item, new List<Tuple<string, string>>());
            }
			#endregion

			#region [실제 Resource 데이터를 넣는다.]
			//"en", "ko-KR", "ja-JP", "zh-CN", "ru-RU", "es-ES"
			lock (ObjLock)
            {

                var keyResource_en = Resources.FirstOrDefault(x => x.Key == "en").Value;
                var keyResource_ko_KR = Resources.FirstOrDefault(x => x.Key == "ko-KR").Value;
                var keyResource_ja_JP = Resources.FirstOrDefault(x => x.Key == "ja-JP").Value;
                var keyResource_zh_CN = Resources.FirstOrDefault(x => x.Key == "zh-CN").Value;
                var keyResource_ru_RU = Resources.FirstOrDefault(x => x.Key == "ru-RU").Value;
				var keyResource_es_ES = Resources.FirstOrDefault(x => x.Key == "es-ES").Value;
				foreach (dynamic item in ResourceData)
                {
                    keyResource_en.Add(new Tuple<string, string>(item.code, item.en));
                    keyResource_ko_KR.Add(new Tuple<string, string>(item.code, item.ko_KR));
                    keyResource_ja_JP.Add(new Tuple<string, string>(item.code, item.ja_JP));
                    keyResource_zh_CN.Add(new Tuple<string, string>(item.code, item.zh_CN));
                    keyResource_ru_RU.Add(new Tuple<string, string>(item.code, item.ru_RU));
					keyResource_es_ES.Add(new Tuple<string, string>(item.code, item.es_ES));
				}

                Resources["en"] = keyResource_en;
                Resources["ko-KR"] = keyResource_ko_KR;
                Resources["ja-JP"] = keyResource_ja_JP;
                Resources["zh-CN"] = keyResource_zh_CN;
                Resources["ru-RU"] = keyResource_ru_RU;
				Resources["es-ES"] = keyResource_es_ES;

			}
            #endregion
        }

        /// <summary>
        /// 국가별 다국어 표현 목록을 가져온다.
        /// </summary>
        /// <param name="country">국가코드.</param>
        /// <param name="category">카테고리.</param>
        /// <returns>다국어 표현 목록.</returns>
        public static dynamic GetLangCategory(string country)
        {
            //country 값으로 인해서 값이 공유가 되는 문제가 발생함.
            var __key = System.Web.HttpContext.Current.Request.Cookies["language"];
            if (__key != null)
            {
                country = __key.Value.vRegExClearScriptTag();
                if (country.vIsEmpty()) country = "en";
                if (!Resources.ContainsKey(country)) return null;
                var cResource = Resources[country] == null ? null : Resources[country];
                return cResource;
            }
            else
            {
                if (!Resources.ContainsKey("en")) return null;
                var cResource = Resources["en"] == null ? null : Resources["en"];
                return cResource;
            }
        }

        /// <summary>
        /// code에맞는 리소스 찾기
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Resource(string code)
        {
            var __key = System.Web.HttpContext.Current.Request.Cookies["language"];

            if (__key != null)
            {
                string country = __key.Value.vRegExClearScriptTag();
                if (country.vIsEmpty()) country = "en";
                if (!Resources.ContainsKey(country)) return null;
                var cResource = Resources[country] == null ? null : Resources[country];
                List<Tuple<string, string>> _currentResource = cResource;
                var data = _currentResource.FirstOrDefault(x => x.Item1.Equals(code, StringComparison.CurrentCultureIgnoreCase));
                if (data != null)
                    return data.Item2.vDefaultValue();
                return "";
            }
            else
            {
                string country = "en";
                if (!Resources.ContainsKey(country)) return null;
                var cResource = Resources[country] == null ? null : Resources[country];
                List<Tuple<string, string>> _currentResource = cResource;
                var data = _currentResource.FirstOrDefault(x => x.Item1.Equals(code, StringComparison.CurrentCultureIgnoreCase));
                if (data != null)
                    return data.Item2.vDefaultValue();
                return "";

            }

        }
    }
}
