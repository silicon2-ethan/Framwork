using System;
using System.Linq;
using System.Text;
using System.Web;

namespace SL.Framework.Utility
{
    public static class QueryStringHelper
    {
        /// <summary>
        /// 현재 Request값이 QueryString을 기준으로 '?' 제외한 QueryString을 생성하여 반환한다.
        /// </summary>
        /// <returns></returns>
        public static string Create()
        {
            return Create("");
        }

        /// <summary>
        /// 현재 Request값이 QueryString을 기준으로 preffix존재시 추가 후 QueryString을 생성하여 반환한다.
        /// </summary>
        /// <param name="preffix">생성QuerySting의 preffix</param>
        /// <returns></returns>
        public static string Create(string preffix)
        {
            return Create(preffix, new string[] {});
        }

        /// <summary>
        /// 현재 Request값이 QueryString을 기준으로 removeParameters 제거 후 preffix존재시 추가 후 QueryString을 생성하여 반환한다.
        /// QueryString의 파라메터는 대소문자 구분 없음.
        /// </summary>
        /// <param name="preffix">생성QuerySting의 preffix</param>
        /// <param name="removeParameters">제거 할 파라메터</param>
        /// <returns></returns>
        public static string Create(string preffix, string[] removeParameters)
        {
            // 반환 파라메터 생성 //
            var preQueryString = new StringBuilder();
            var querys = HttpContext.Current.Request.QueryString;

            // 제거 파라메터가 null일 경우 //
            if (removeParameters == null)
                removeParameters = new string[] {};

            // 제거 파라메터를 제외한 파라메터 생성 //
            foreach (
                var name in
                    querys.Cast<string>()
                        .Where(
                            name => removeParameters.Count(n => n.Equals(name, StringComparison.OrdinalIgnoreCase)) == 0)
                )
            {
                preQueryString.Append("&" + name + "=" + HttpUtility.UrlEncode(querys[name]));
            }

            // 반환 QueryString 최종 생성 후 preffix존재 시 포함하여 재생성 한다. //
            var queryString = preQueryString.ToString();
            if (queryString.Length > 0)
                queryString = queryString.Substring(1);

            // preffix 존재시 추가 후 반환 //
            return StringHelper.AddPrefix(queryString, preffix);
        }

        /// <summary>
        /// 현재 QueryString중 원하는 파라메터만 QueryString생성 후 반환한다.
        /// </summary>
        /// <param name="createParameters">생성파라메터명</param>
        /// <returns></returns>
        public static string Create(string[] createParameters)
        {
            return Create(createParameters, "");
        }

        /// <summary>
        /// 현재 QueryString중 원하는 파라메터만 QueryString생성 후 preffix를 포함하여 반환한다.
        /// </summary>
        /// <param name="createParameters">생성파라메터명</param>
        /// <param name="preffix">preffix</param>
        /// <returns></returns>
        public static string Create(string[] createParameters, string preffix)
        {
            var preQueryString = new StringBuilder();
            var querys = HttpContext.Current.Request.QueryString;

            foreach (
                var name in
                    querys.Cast<string>()
                        .Where(
                            name => createParameters.Count(n => n.Equals(name, StringComparison.OrdinalIgnoreCase)) > 0)
                )
            {
                preQueryString.Append("&" + name + "=" + HttpUtility.UrlEncode(querys[name]));
            }

            // 반환 QueryString 최종 생성 후 preffix존재 시 포함하여 재생성 한다. //
            var queryString = preQueryString.ToString();
            if (queryString.Length > 0)
                queryString = queryString.Substring(1);

            // preffix 존재시 추가 후 반환 //
            return StringHelper.AddPrefix(queryString, preffix);
        }

        /// <summary>
        /// 문자열 쿼리스트링 중 제거파라메터 처리후 쿼리스트링 값 반환
        /// </summary>
        /// <param name="queryParams">QeuryString</param>
        /// <param name="removeParameters">제거 파라메터</param>
        /// <param name="preffix">preffix</param>
        /// <returns></returns>
        public static string Create(string queryParams, string[] removeParameters, string preffix)
        {
            var preQueryString = new StringBuilder();
            if (!string.IsNullOrEmpty(queryParams))
            {
                if (removeParameters == null)
                    removeParameters = new string[] {};

                if (queryParams.StartsWith("?"))
                    queryParams = queryParams.Substring(1);

                foreach (
                    var paramInfos in
                        queryParams.Split("&".ToCharArray())
                            .Select(paramInfo => paramInfo.Split("=".ToCharArray()))
                            .Where(
                                paramInfos =>
                                    removeParameters.Count(
                                        n => n.Equals(paramInfos[0], StringComparison.OrdinalIgnoreCase)) == 0))
                {
                    preQueryString.Append("&" + paramInfos[0] + "=" + paramInfos[1]);
                }
            }

            // 반환 QueryString 최종 생성 후 preffix존재 시 포함하여 재생성 한다. //
            var queryString = preQueryString.ToString();
            if (queryString.Length > 0)
                queryString = queryString.Substring(1);

            // preffix 존재시 추가 후 반환 //
            return StringHelper.AddPrefix(queryString, preffix);
        }

        /// <summary>
        /// QueryString값을 재생성 한다.
        /// </summary>
        /// <param name="querys">기본 Query파라메트</param>
        /// <param name="removeParameters">제거파라메터</param>
        /// <param name="preffix">preffix</param>
        /// <returns></returns>
        public static string ParamCreate(string querys, string[] removeParameters, string preffix)
        {
            var preQueryString = new StringBuilder();
            if (querys.StartsWith("?"))
                querys = querys.Substring(1);

            if (removeParameters == null)
                removeParameters = new string[] {};

            foreach (
                var param in
                    querys.Split("&".ToCharArray())
                        .Select(query => query.Split("=".ToCharArray()))
                        .Where(
                            param =>
                                removeParameters.Count(n => n.Equals(param[0], StringComparison.OrdinalIgnoreCase)) == 0)
                )
            {
                preQueryString.Append("&" + param[0] + "=" + HttpUtility.UrlEncode(param[1]));
            }

            var result = preQueryString.ToString();
            return string.IsNullOrWhiteSpace(result) ? "" : preffix + result.Substring(1);
        }
    }
}