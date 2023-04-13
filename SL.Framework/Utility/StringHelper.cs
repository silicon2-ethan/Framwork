using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using SL.Framework.Extension;

namespace SL.Framework.Utility
{
    public static class IEnumerableExtensions
    {

        public static IEnumerable<t> Randomize<t>(this IEnumerable<t> target)
        {
            Random r = new Random();

            return target.OrderBy(x => (r.Next()));
        }
    }

    public static class StringHelper
    {
        /// <summary>
        /// 대소문자 구분 무시하여 replace할 경우 .netstandart 버젼 사용하지 않는 경우
        /// </summary>
        /// <param name="input"></param>
        /// <param name="search"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string vReplaceCaseInsensitive(this string input, string search, string replacement)
        {
            string result = Regex.Replace(
                input,
                Regex.Escape(search),
                replacement.Replace("$", "$$"),
                RegexOptions.IgnoreCase
            );
            return result;
        }

        /// <summary>
        /// 램덤문자열 생성 0~9, A~Z, a~z
        /// </summary>
        /// <param name="length">생성할 코드 길이</param>
        /// <returns></returns>
        public static string GetRandomString(int length)
        {
            // 생성코드 //
            var result = String.Empty;

            // 코드생성에 사용 될 문자열 //
            var codeArray = new string[62]
            {
                "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
                "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
                "U", "V", "W", "X", "Y", "Z",
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j",
                "k", "l", "m", "n", "o", "p", "q", "r", "s", "t",
                "u", "v", "w", "x", "y", "z"
            };

            // 요청코드 길이만큼 코드 생성 //
            var random = new Random();
            var i = 0;
            while (i != length)
            {
                i++;
                result += codeArray[random.Next(codeArray.Length)];
            }

            return result;
        }

        /// <summary>
        /// 문자열정렬을 랜덤정렬 한다.
        /// </summary>
        /// <param name="source">문자열배열값</param>
        /// <returns></returns>
        public static string[] RandomSort(string[] source)
        {
            var maxValue = source.Length;
            var rnd = new Random();
            for (var i = 0; i < maxValue; i++)
            {
                var tmpValue = rnd.Next(maxValue - i) + i;
                var swapValue = source[i];
                source[i] = source[tmpValue];
                source[tmpValue] = swapValue;
            }
            return source;
        }

        /// <summary>
        /// 입력값에 값이 존재하면 prefix를 연결하여 반환하고, 없으면 ""를 반환한다.
        /// </summary>
        /// <param name="value">입력값</param>
        /// <param name="prefix">prefix</param>
        /// <returns></returns>
        public static string AddPrefix(string value, string prefix)
        {
            return !string.IsNullOrEmpty(value) ? prefix + value : "";
        }

        /// <summary>
        /// HTML 모든 태그 제거
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveHtmlTag(this string value)
        {
            // script, style은 태그 내용 함께 삭제
            var pattern = "<(\\s*)script.*?/script(\\s*)>|<(\\s*)style.*?/style(\\s*)>";
            var reg = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            value = reg.Replace(value, string.Empty);

            // 기타 모든 태그 앨리먼트 삭제
            pattern = "<(/?)(\\s*)([a-z]+).*?>";
            reg = new Regex(pattern, RegexOptions.IgnoreCase);
            value = reg.Replace(value, string.Empty);

            // ＆nbsp; ＆amp; ＆lt; ＆gt; ＆quot; 코드문자 제거
            pattern = "&(nbsp|amp|lt|gt|quot);";
            reg = new Regex(pattern, RegexOptions.IgnoreCase);

            return reg.Replace(value, string.Empty).Trim();
        }

        /// <summary>
        /// source를 separator로 Split하여 사용위치부터 연결문자열로 연결하여 반환
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="separator">separator</param>
        /// <param name="index">연결위치</param>
        /// <param name="cstring">연결문자열</param>
        /// <returns></returns>
        public static string SplitToString(string source, string separator, int index, string cstring)
        {
            var i = 0;
            var result = "";
            foreach (var s in source.Split(separator.ToCharArray()))
            {
                if (i >= index)
                    result = result + string.Format("{0}{1}", cstring, s);

                i++;
            }
            if (!string.IsNullOrWhiteSpace(result))
                result = result.Substring(cstring.Length);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="matchstring"></param>
        /// <returns></returns>
        public static bool IsMatch(this string str, string matchstring)
        {
            return new Regex(matchstring).IsMatch(str.vDefaultValue());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="matchstring"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static bool IsMatch(this string str, string matchstring, RegexOptions option)
        {
            return new Regex(matchstring, option).IsMatch(str.vDefaultValue());
        }

        /// <summary>
        /// 정규표현식
        /// </summary>
        /// <param name="value"></param>
        /// <param name="exP">정규식</param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static string vRegExText(this string value, string exP = "[a-zA-Z0-9_*-.]{5,}",
            RegexOptions option = RegexOptions.IgnoreCase)
        {
            var exValue = value.vDefaultValue();
            var ex = new Regex(exP, option);
            return ex.IsMatch(exValue) ? exValue : "";
        }

        /// <summary>
        /// 로그인한 사용자 아이디
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string vRegExLoginID(this string value)
        {
            return value.vDefaultValue().vRegExText(exP: "[a-zA-Z0-9_*-.]{5,}");
        }

        /// <summary>
        /// 전화번호 체크
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string vRegExPhoneNumber(this string value)
        {
            return value.vDefaultValue().vRegExText(exP: "^0[0-9]+");
        }

        /// <summary>
        /// 영문/숫자 체크
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string vRegExAlphabetNumber(this string value)
        {
            return value.vDefaultValue().vRegExText(exP: @"^[0-9a-zA-Z]");
        }
        /// <summary>
        /// 국제번호 및 핸드폰번호 체크
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string vRegExNationNumber(this string value)
        {
            return value.vDefaultValue().vRegExText(exP: @"^((\+\d{1,3}(-| )?\(?\d\)?(-| )?\d{1,5})|(\(?\d{2,6}\)?))(-| )?(\d{3,4})(-| )?(\d{4})(( x| ext)\d{1,5}){0,1}$");
        }

        /// <summary>
        /// 숫자여부 체크
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string vRegExNumber(this string value)
        {
            return value.vDefaultValue().vRegExText(exP: @"^[0-9]");
        }

        /// <summary>
        /// 지역코드 체크
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string vRegExStatusCode(this string value)
        {
            return value.vDefaultValue().vRegExText(exP: @"^[0-9]+$");
        }

        /// <summary>
        /// 태그를 제거하고 넘겨준다.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string vRegExClearScriptTag(this string html)
        {
            if (html.vIsEmpty()) return "";
            string tmp = Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
            string pattern = "&(nbsp|amp|lt|gt|quot);";
            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
            return reg.Replace(tmp, string.Empty).Trim();
        }

        /// <summary>
        /// 이미지 src 정보 불러오기 처리
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string vRegExGetImgSrc(this string value)
        {
            string retunValue = "";
            Regex rxImages = new Regex("<img.+?src=[\"'](.+?)[\"'].+?>",
                               RegexOptions.IgnoreCase & RegexOptions.IgnorePatternWhitespace);
            MatchCollection mc = rxImages.Matches(value);
            foreach (Match m in mc)
            {
                retunValue = m.Groups[1].Value;
            }
            return retunValue;
        }

        /// <summary>
        /// 이미지 태그를 제거하고 넘겨준다.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string vRegExClearImageTag(this string html)
        {
            if (html.vIsEmpty()) return "";
            string tmp = Regex.Replace(html, @"<img\s[^>]*>(?:\s*?</img>)?", string.Empty);
            string pattern = "&(nbsp|amp|lt|gt|quot);";
            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
            return reg.Replace(tmp, string.Empty).Trim();
        }


        /// <summary>
        /// 영문자만 허용한다.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string vRegExClearAlphabet(this string html)
        {
            string tmp = Regex.Replace(html, @"[^a-zA-Z]", string.Empty);
            string pattern = "&(nbsp|amp|lt|gt|quot);";
            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
            return reg.Replace(tmp, string.Empty).Trim();
        }

        /// <summary>
        /// 메일 인지 여부 체크
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool vIsRegExEmail(this string value)
        {
            return value.IsMatch(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");
        }

        public static string vLeft(this string str, int Length)
        {
            if (str.Length < Length)
                Length = str.Length;
            return str.Substring(0, Length);
        }

        public static string vRight(this string str, int Length)
        {
            if (str.Length < Length)
                Length = str.Length;
            return str.Substring(str.Length - Length, Length);

        }

        /// <summary>
        /// Sp에서 사용할 input 파라미터 가공.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="IsRoot"></param>
        /// <returns></returns>
        public static string GetXmlInputParameters(this Dictionary<string, string> value, bool IsRoot)
        {
            Func<string, string> TextToXml = ((val) =>
            {
                if (val is null) return val;
                return val.Replace("&", "&amp;")
                             .Replace("<", "&lt;")
                             .Replace(">", "&gt;")
                             .Replace("'", "&apos;")
                             .Replace("\"", "&quot;")
                             .Replace("\r\n", "&#10;")
                             .Replace("\n", "&#10;")
                             .Replace("\r", "&#10;");
            });

            List<string> data = new List<string>();
            data.Add("<data");
            foreach (string item in value.Keys)
            {
                if (value[item] is null) continue;
                data.Add(string.Format("{0}=\"{1}\"", item, TextToXml(value[item])));
            }

            data.Add("/>");
            if (IsRoot)
            {
                data.Insert(0, "<root>");
                data.Add("</root>");
            }
            return string.Join(" ", data.ToArray());
        }

        /// <summary>
        /// Sp에서 사용할 input 파라미터 가공.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="IsRoot"></param>
        /// <returns></returns>
        public static string GetXmlInputParameters(this List<Dictionary<string, string>> value, bool IsRoot)
        {
            List<string> data = new List<string>();

            foreach (Dictionary<string, string> item in value)
            {
                data.Add(item.GetXmlInputParameters(false));
            }

            if (IsRoot)
            {
                data.Insert(0, "<root>");
                data.Add("</root>");
            }
            return string.Join(" ", data.ToArray());
        }


        /// <summary>
        /// GMT시간 기준으로 오늘일자 가지고 오기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TodayByGMTTime(this string value, int hours, string format = "yyyy-MM-dd")
        {
            return DateTime.UtcNow.AddHours(hours).ToString(format);
        }

        #region [C-query]

        #region [MS-sql]
        public static string InsertSqlQueryForMssql<T>(T t) where T : class
        {
            string returnValue = "";
            List<string> lstParam = new List<string>();
            List<object> lstValues = new List<object>();

            for (int i = 0; i < t.GetType().GetProperties().Length; i++)
            {
                var p = t.GetType().GetProperties()[i];
                if (p.PropertyType == typeof(string)
                        || p.PropertyType == typeof(DateTime))
                {
                    lstValues.Add("'" + (p.GetValue(t, null) ?? "") + "'");
                    lstParam.Add(p.Name);
                }
                else
                {
                    if (p.GetValue(t, null) != null)
                    {
                        lstParam.Add(p.Name);
                        lstValues.Add((p.GetValue(t, null) ?? default(T)));
                    }
                }
            }

            const string insertQuery = "insert into {0}({1}) values({2});";

            returnValue = string.Format(insertQuery,
                                                t.GetType().Name,
                                                string.Join(",", lstParam.ToArray()),
                                                string.Join(",", lstValues.ToArray()));
            return returnValue;
        }

        public static string ExistsSelectQueryForMssql<T>(T t, List<T> wherePhrase) where T : class
        {
            string returnValue = "";
            List<object> lstValues = new List<object>();

            foreach (var item in wherePhrase)
            {
                for (int i = 0; i < item.GetType().GetProperties().Length; i++)
                {
                    var p = item.GetType().GetProperties()[i];

                    if (p.PropertyType == typeof(string)
                            || p.PropertyType == typeof(DateTime))
                    {
                        if (p.GetValue(item, null) != null)
                            lstValues.Add($" and {p.Name} ='" + (p.GetValue(item, null) ?? "") + "'");
                    }
                    else
                    {
                        if (p.GetValue(item, null) != null)
                            lstValues.Add($" and {p.Name} = " + (p.GetValue(item, null) ?? ""));
                    }
                }
            }

            string selectQuery = "select  top 1 1 from {0}  where (1=1) {1};";

            returnValue = string.Format(selectQuery,
                                                t.GetType().Name,
                                                string.Join(" ", lstValues.ToArray()));
            return returnValue;
        }

        public static string SelectSqlQueryForMssql<T>(T t, List<T> wherePhrase) where T : class
        {
            string returnValue = "";
            List<object> lstValues = new List<object>();
            List<string> lstParam = new List<string>();

            for (int i = 0; i < t.GetType().GetProperties().Length; i++)
            {
                var p = t.GetType().GetProperties()[i];
                lstParam.Add(p.Name);
            }

            foreach (var item in wherePhrase)
            {
                for (int i = 0; i < item.GetType().GetProperties().Length; i++)
                {
                    var p = item.GetType().GetProperties()[i];

                    if (p.PropertyType == typeof(string)
                            || p.PropertyType == typeof(DateTime))
                    {
                        if (p.GetValue(item, null) != null)
                            lstValues.Add($" and {p.Name} ='" + (p.GetValue(item, null) ?? "") + "'");
                    }
                    else
                    {
                        if (p.GetValue(item, null) != null)
                            lstValues.Add($" and {p.Name} = " + (p.GetValue(item, null) ?? ""));
                    }
                }
            }

            string selectQuery = "select  {2} from {0}  where (1=1) {1};";

            returnValue = string.Format(selectQuery,
                                                t.GetType().Name,
                                                string.Join(" ", lstValues.ToArray()),
                                                string.Join(", ", lstParam.ToArray()));
            return returnValue;
        }

        public static string UpdateSqlQueryForMssql<T>(T t, List<T> wherePhrase) where T : class
        {
            string returnValue = "";
            List<object> lstValues = new List<object>();
            List<string> lstParam = new List<string>();

            List<Tuple<string, object>> wherePhraseKeyValues = new List<Tuple<string, object>>();
            List<Tuple<string, object>> updatePhraseKeyValues = new List<Tuple<string, object>>();


            for (int i = 0; i < t.GetType().GetProperties().Length; i++)
            {
                var p = t.GetType().GetProperties()[i];
                if (p.PropertyType == typeof(string)
                        || p.PropertyType == typeof(DateTime))
                {
                    if (p.GetValue(t, null) != null)
                    {
                        //lstParam.Add($" {p.Name} ='" + (p.GetValue(t, null) ?? "") + "'");
                        updatePhraseKeyValues.Add(new Tuple<string, object>(p.Name, "'" + (p.GetValue(t, null) ?? "") + "'"));
                    }
                }
                else
                {
                    if (p.GetValue(t, null) != null)
                    {
                        //lstParam.Add($" {p.Name} =" + (p.GetValue(t, null) ?? default(T)));
                        updatePhraseKeyValues.Add(new Tuple<string, object>(p.Name, "" + (p.GetValue(t, null) ?? default(T)) + ""));
                    }
                }
            }

            foreach (var item in wherePhrase)
            {
                for (int i = 0; i < item.GetType().GetProperties().Length; i++)
                {
                    var p = item.GetType().GetProperties()[i];

                    if (p.PropertyType == typeof(string)
                            || p.PropertyType == typeof(DateTime))
                    {
                        if (p.GetValue(item, null) != null)
                        {
                            //lstValues.Add($" and {p.Name} ='" + (p.GetValue(item, null) ?? "") + "'");
                            wherePhraseKeyValues.Add(new Tuple<string, object>(p.Name, "'" + (p.GetValue(item, null) ?? "") + "'"));
                        }
                    }
                    else
                    {
                        if (p.GetValue(item, null) != null)
                        {
                            //lstValues.Add($" and {p.Name} = " + (p.GetValue(item, null) ?? default(T)));
                            wherePhraseKeyValues.Add(new Tuple<string, object>(p.Name, "" + (p.GetValue(item, null) ?? default(T)) + ""));
                        }
                    }
                }
            }

            string selectQuery = "update  {0} set {2} where (1=1) {1};";

            foreach (var item in wherePhraseKeyValues)
            {
                var searchOne = updatePhraseKeyValues.FirstOrDefault(x => x.Item1.Equals(item.Item1));
                if (searchOne != null)
                {
                    updatePhraseKeyValues.Remove(searchOne);
                }
            }

            foreach (var item in updatePhraseKeyValues)
            {
                lstParam.Add($"{item.Item1}={item.Item2}");
            }

            foreach (var item in wherePhraseKeyValues)
            {
                lstValues.Add($"and {item.Item1}={item.Item2}");
            }

            returnValue = string.Format(selectQuery,
                                                t.GetType().Name,
                                                string.Join(" ", lstValues.ToArray()),
                                                string.Join(", ", lstParam.ToArray()));
            return returnValue;
        }

        public static string DeleteQueryForMssql<T>(T t, List<T> wherePhrase) where T : class
        {
            string returnValue = "";
            List<object> lstValues = new List<object>();

            foreach (var item in wherePhrase)
            {
                for (int i = 0; i < item.GetType().GetProperties().Length; i++)
                {
                    var p = item.GetType().GetProperties()[i];

                    if (p.PropertyType == typeof(string)
                            || p.PropertyType == typeof(DateTime))
                    {
                        if (p.GetValue(item, null) != null)
                            lstValues.Add($" and {p.Name} ='" + (p.GetValue(item, null) ?? "") + "'");
                    }
                    else
                    {
                        if (p.GetValue(item, null) != null)
                            lstValues.Add($" and {p.Name} = " + (p.GetValue(item, null) ?? ""));
                    }
                }
            }

            string selectQuery = "delete from {0}  where (1=1) {1};";

            returnValue = string.Format(selectQuery,
                                                t.GetType().Name,
                                                string.Join(" ", lstValues.ToArray()));
            return returnValue;
        }
        #endregion

        #region [Mysql]
        /// <summary>
        /// class 넘겨서 insert 쿼리를 만든다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string InsertSqlQueryForMysql<T>(T t) where T : class
        {
            string returnValue = "";
            List<string> lstParam = new List<string>();
            List<object> lstValues = new List<object>();

            for (int i = 0; i < t.GetType().GetProperties().Length; i++)
            {
                var p = t.GetType().GetProperties()[i];
                if (p.PropertyType == typeof(string)
                        || p.PropertyType == typeof(DateTime))
                {
                    lstValues.Add("'" + (p.GetValue(t, null) ?? "") + "'");
                    lstParam.Add(p.Name);
                }
                else
                {
                    if (p.GetValue(t, null) != null)
                    {
                        lstParam.Add(p.Name);
                        lstValues.Add((p.GetValue(t, null) ?? default(T)));
                    }
                }
            }

            const string insertQuery = "insert into {0}({1}) values({2});";

            returnValue = string.Format(insertQuery,
                                                t.GetType().Name,
                                                string.Join(",", lstParam.ToArray()),
                                                string.Join(",", lstValues.ToArray()));
            return returnValue;
        }

        public static string ExistsSelectQueryForMysql<T>(T t, List<T> wherePhrase) where T : class
        {
            string returnValue = "";
            List<object> lstValues = new List<object>();

            foreach (var item in wherePhrase)
            {
                for (int i = 0; i < item.GetType().GetProperties().Length; i++)
                {
                    var p = item.GetType().GetProperties()[i];

                    if (p.PropertyType == typeof(string)
                            || p.PropertyType == typeof(DateTime))
                    {
                        if (p.GetValue(item, null) != null)
                            lstValues.Add($" and {p.Name} ='" + (p.GetValue(item, null) ?? "") + "'");
                    }
                    else
                    {
                        if (p.GetValue(item, null) != null)
                            lstValues.Add($" and {p.Name} = " + (p.GetValue(item, null) ?? ""));
                    }
                }
            }

            string selectQuery = "select  1 from {0}  where (1=1) {1} limit 1;";

            returnValue = string.Format(selectQuery,
                                                t.GetType().Name,
                                                string.Join(" ", lstValues.ToArray()));
            return returnValue;
        }

        public static string SelectSqlQueryForMysql<T>(T t, List<T> wherePhrase) where T : class
        {
            string returnValue = "";
            List<object> lstValues = new List<object>();
            List<string> lstParam = new List<string>();

            for (int i = 0; i < t.GetType().GetProperties().Length; i++)
            {
                var p = t.GetType().GetProperties()[i];
                lstParam.Add(p.Name);
            }

            foreach (var item in wherePhrase)
            {
                for (int i = 0; i < item.GetType().GetProperties().Length; i++)
                {
                    var p = item.GetType().GetProperties()[i];

                    if (p.PropertyType == typeof(string)
                            || p.PropertyType == typeof(DateTime))
                    {
                        if (p.GetValue(item, null) != null)
                            lstValues.Add($" and {p.Name} ='" + (p.GetValue(item, null) ?? "") + "'");
                    }
                    else
                    {
                        if (p.GetValue(item, null) != null)
                            lstValues.Add($" and {p.Name} = " + (p.GetValue(item, null) ?? ""));
                    }
                }
            }

            string selectQuery = "select  {2} from {0}  where (1=1) {1};";

            returnValue = string.Format(selectQuery,
                                                t.GetType().Name,
                                                string.Join(" ", lstValues.ToArray()),
                                                string.Join(", ", lstParam.ToArray()));
            return returnValue;
        }

        public static string UpdateSqlQueryForMysql<T>(T t, List<T> wherePhrase) where T : class
        {
            string returnValue = "";
            List<object> lstValues = new List<object>();
            List<string> lstParam = new List<string>();

            List<Tuple<string, object>> wherePhraseKeyValues = new List<Tuple<string, object>>();
            List<Tuple<string, object>> updatePhraseKeyValues = new List<Tuple<string, object>>();


            for (int i = 0; i < t.GetType().GetProperties().Length; i++)
            {
                var p = t.GetType().GetProperties()[i];
                if (p.PropertyType == typeof(string)
                        || p.PropertyType == typeof(DateTime))
                {
                    if (p.GetValue(t, null) != null)
                    {
                        //lstParam.Add($" {p.Name} ='" + (p.GetValue(t, null) ?? "") + "'");
                        updatePhraseKeyValues.Add(new Tuple<string, object>(p.Name, "'" + (p.GetValue(t, null) ?? "") + "'"));
                    }
                }
                else
                {
                    if (p.GetValue(t, null) != null)
                    {
                        //lstParam.Add($" {p.Name} =" + (p.GetValue(t, null) ?? default(T)));
                        updatePhraseKeyValues.Add(new Tuple<string, object>(p.Name, "" + (p.GetValue(t, null) ?? default(T)) + ""));
                    }
                }
            }

            foreach (var item in wherePhrase)
            {
                for (int i = 0; i < item.GetType().GetProperties().Length; i++)
                {
                    var p = item.GetType().GetProperties()[i];

                    if (p.PropertyType == typeof(string)
                            || p.PropertyType == typeof(DateTime))
                    {
                        if (p.GetValue(item, null) != null)
                        {
                            //lstValues.Add($" and {p.Name} ='" + (p.GetValue(item, null) ?? "") + "'");
                            wherePhraseKeyValues.Add(new Tuple<string, object>(p.Name, "'" + (p.GetValue(item, null) ?? "") + "'"));
                        }
                    }
                    else
                    {
                        if (p.GetValue(item, null) != null)
                        {
                            //lstValues.Add($" and {p.Name} = " + (p.GetValue(item, null) ?? default(T)));
                            wherePhraseKeyValues.Add(new Tuple<string, object>(p.Name, "" + (p.GetValue(item, null) ?? default(T)) + ""));
                        }
                    }
                }
            }

            string selectQuery = "update  {0} set {2} where (1=1) {1};";

            foreach (var item in wherePhraseKeyValues)
            {
                var searchOne = updatePhraseKeyValues.FirstOrDefault(x => x.Item1.Equals(item.Item1));
                if (searchOne != null)
                {
                    updatePhraseKeyValues.Remove(searchOne);
                }
            }

            foreach (var item in updatePhraseKeyValues)
            {
                lstParam.Add($"{item.Item1}={item.Item2}");
            }

            foreach (var item in wherePhraseKeyValues)
            {
                lstValues.Add($"and {item.Item1}={item.Item2}");
            }

            returnValue = string.Format(selectQuery,
                                                t.GetType().Name,
                                                string.Join(" ", lstValues.ToArray()),
                                                string.Join(", ", lstParam.ToArray()));
            return returnValue;
        }

        public static string DeleteQueryForMysql<T>(T t, List<T> wherePhrase) where T : class
        {
            string returnValue = "";
            List<object> lstValues = new List<object>();

            foreach (var item in wherePhrase)
            {
                for (int i = 0; i < item.GetType().GetProperties().Length; i++)
                {
                    var p = item.GetType().GetProperties()[i];

                    if (p.PropertyType == typeof(string)
                            || p.PropertyType == typeof(DateTime))
                    {
                        if (p.GetValue(item, null) != null)
                            lstValues.Add($" and {p.Name} ='" + (p.GetValue(item, null) ?? "") + "'");
                    }
                    else
                    {
                        if (p.GetValue(item, null) != null)
                            lstValues.Add($" and {p.Name} = " + (p.GetValue(item, null) ?? ""));
                    }
                }
            }

            string selectQuery = "delete from {0}  where (1=1) {1};";

            returnValue = string.Format(selectQuery,
                                                t.GetType().Name,
                                                string.Join(" ", lstValues.ToArray()));
            return returnValue;
        }
        #endregion

        #endregion

        /// <summary>
        /// Request 객체 get/post 방식으로 문자열 가공
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ConvertToStringRequestParams<T>(T t) where T : class
        {
            List<string> lstValues = new List<string>();

            for (int i = 0; i < t.GetType().GetProperties().Length; i++)
            {
                var p = t.GetType().GetProperties()[i];
                if (p.PropertyType == typeof(string)
                        || p.PropertyType == typeof(DateTime))
                {
                    if (p.GetValue(t, null) != null)
                        lstValues.Add($"{p.Name}={(p.GetValue(t, null) ?? "")}");
                }
                else
                {
                    if (p.GetValue(t, null) != null)
                        lstValues.Add($"{p.Name}={(p.GetValue(t, null))}");
                }
            }
            return (lstValues.Count <= 0) ? "" : string.Join("&", lstValues.ToArray());
        }

        public static DataTable ConvertHashtableListToDataTable(IList<Hashtable> list, bool bForXls)
        {
            //create an instance of DataTable
            var dataTable = new DataTable("Export");

            if (list.Count > 0)
            {
                int i = 0;
                foreach (Hashtable ht in list)
                {
                    SortedList sl = new SortedList(ht);
                    if (i == 0)
                    {
                        //fill the columns in the DataTable
                        foreach (DictionaryEntry entry in sl)
                        {
                            dataTable.Columns.Add((bForXls ? entry.Key.ToString().Substring(4) : entry.Key.ToString()), typeof(string));
                        }

                    }
                    i++;

                    //create a new DataRow in the DataTable	
                    DataRow dr = dataTable.NewRow();

                    //fill the new row in the DataTable
                    foreach (DictionaryEntry entry in sl)
                    {
                        dr[(bForXls ? entry.Key.ToString().Substring(4) : entry.Key.ToString())] = (entry.Value == null ? "" : entry.Value.ToString());
                    }
                    //add the filled up row to the DataTable
                    dataTable.Rows.Add(dr);
                }
            }

            //return the DataTable
            return dataTable;
        }


        /// <summary>
        /// DataTable 데이터 및 컬럼 보정하기
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="sp_name"></param>
        /// <param name="sp_mode"></param>
        /// <returns></returns>
        public static DataTable CorrectionDataTable(DataTable dtSource, string sp_name, string sp_mode)
        {
            DataTable dtCorrection = dtSource.Copy();

            //상품 -> 상품메뉴에서 Excel 다운로드 할 경우
            if (sp_name == "USP_PROD" && sp_mode == "R91")
            {
                dtCorrection.Columns.Add("exist_prod_image", typeof(System.String));

                dtCorrection.Select().ToList<DataRow>().ForEach(row =>
                {
                    row["exist_prod_image"] = row.GetInt32("cnts_cnt", 0) > 0 ? "Y" : "N";
                });

                dtCorrection.Columns.Remove("cnts_cnt");
                dtCorrection.Columns["exist_prod_image"].ColumnName = "image";
            }
            return dtCorrection;
        }

        /// <summary>
        /// 숫자인지 여부
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string value)
        {
            int n = 0;
            return int.TryParse(value, out n);
        }
    }

    public static class DataTableX
    {
        public static IEnumerable<dynamic> AsDynamicEnumerable(this DataTable table)
        {
            // Validate argument here..

            return table.AsEnumerable().Select(row => new DynamicRow(row));
        }

        private sealed class DynamicRow : DynamicObject
        {
            private readonly DataRow _row;

            internal DynamicRow(DataRow row) { _row = row; }

            // Interprets a member-access as an indexer-access on the 
            // contained DataRow.
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                var retVal = _row.Table.Columns.Contains(binder.Name);
                result = retVal ? _row[binder.Name] : null;
                return retVal;
            }
        }
    }

    /// <summary>
    /// 한글 헬퍼
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <example>
    ///Console.WriteLine($"강동구 : {HangulHelper.IsHangul("강동구")}");
    /// Console.WriteLine($"강동A  : {HangulHelper.IsHangul("강동A")}");
    /// Console.WriteLine($"강     : {HangulHelper.IsHangul('강')}");
    /// Console.WriteLine($"A      : {HangulHelper.IsHangul('A')}");
    /// Console.WriteLine();
    /// char[] elementArray = HangulHelper.DivideHangul('강');
    /// char[] elementArray1 = HangulHelper.DivideHangul('도');
    /// char[] elementArray2 = HangulHelper.DivideHangul('길');
    /// </example>
    public static class HangulHelper
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// 초성 수
        /// </summary>
        private const int INITIAL_COUNT = 19;

        /// <summary>
        /// 중성 수
        /// </summary>
        private const int MEDIAL_COUNT = 21;

        /// <summary>
        /// 종성 수
        /// </summary>
        private const int FINAL_COUNT = 28;

        /// <summary>
        /// 한글 유니코드 시작 인덱스
        /// </summary>
        private const int HANGUL_UNICODE_START_INDEX = 0xac00;

        /// <summary>
        /// 한글 유니코드 종료 인덱스
        /// </summary>
        private const int HANGUL_UNICODE_END_INDEX = 0xD7A3;

        /// <summary>
        /// 초성 시작 인덱스
        /// </summary>
        private const int INITIAL_START_INDEX = 0x1100;

        /// <summary>
        /// 중성 시작 인덱스
        /// </summary>
        private const int MEDIAL_START_INDEX = 0x1161;

        /// <summary>
        /// 종성 시작 인덱스
        /// </summary>
        private const int FINAL_START_INDEX = 0x11a7;

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Static
        //////////////////////////////////////////////////////////////////////////////// Public

        #region 한글 여부 구하기 - IsHangul(char source)

        /// <summary>
        /// 한글 여부 구하기
        /// </summary>
        /// <param name="source">소스 문자</param>
        /// <returns>한글 여부</returns>
        public static bool IsHangul(char source)
        {
            if (HANGUL_UNICODE_START_INDEX <= source && source <= HANGUL_UNICODE_END_INDEX)
            {
                return true;
            }

            return false;
        }

        #endregion
        #region 한글 여부 구하기 - IsHangul(string source)

        /// <summary>
        /// 한글 여부 구하기
        /// </summary>
        /// <param name="source">소스 문자열</param>
        /// <returns>한글 여부</returns>
        public static bool IsHangul(string source)
        {
            bool result = false;

            for (int i = 0; i < source.Length; i++)
            {
                if (HANGUL_UNICODE_START_INDEX <= source[i] && source[i] <= HANGUL_UNICODE_END_INDEX)
                {
                    result = true;
                }
                else
                {
                    result = false;

                    break;
                }
            }

            return result;
        }

        #endregion
        #region 한글 나누기 - DivideHangul(source)

        /// <summary>
        /// 한글 나누기
        /// </summary>
        /// <param name="source">소스 한글 문자</param>
        /// <returns>분리된 자소 배열</returns>
        public static char[] DivideHangul(char source)
        {
            char[] elementArray = null;

            if (IsHangul(source))
            {
                int index = source - HANGUL_UNICODE_START_INDEX;

                int initial = INITIAL_START_INDEX + index / (MEDIAL_COUNT * FINAL_COUNT);
                int medial = MEDIAL_START_INDEX + (index % (MEDIAL_COUNT * FINAL_COUNT)) / FINAL_COUNT;
                int final = FINAL_START_INDEX + index % FINAL_COUNT;

                if (final == 4519)
                {
                    elementArray = new char[2];

                    elementArray[0] = (char)initial;
                    elementArray[1] = (char)medial;
                }
                else
                {
                    elementArray = new char[3];

                    elementArray[0] = (char)initial;
                    elementArray[1] = (char)medial;
                    elementArray[2] = (char)final;
                }
            }

            return elementArray;
        }

        #endregion
    }
}