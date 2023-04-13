using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SL.Framework.Utility
{
    public static class HttpContextHelper
    {
        public static string Post = "POST", Get = "GET", Put = "PUT", Delete = "DELETE";

        /// <summary>
        /// HTTP CONTEXT
        /// </summary>
        private static HttpContext Context
        {
            get { return HttpContext.Current; }
        }

        #region IsHttps

        /// <summary>
        /// HTTPS 요청 여부 반환
        /// RETURN : true, false
        /// </summary>
        public static bool IsHttps
        {
            get { return Context.Request.IsSecureConnection; }
        }

        /// <summary>
        /// 개발자환경인지 여부
        /// </summary>
        public static bool IsLocalHost
        {
            get { return String.Compare(Context.Request.Url.Host, "localhost", StringComparison.OrdinalIgnoreCase) == 0; }
        }

        #endregion

        #region RequestMethod

        /// <summary>
        /// REQUEST METHOD 반환
        /// RETURN : GET, POST
        /// </summary>
        public static string RequestMethod
        {
            get { return Context.Request.HttpMethod.ToUpper(); }
        }

        #endregion

        #region RequestHeaders

        /// <summary>
        /// REQUEST HEADERS 반환
        /// </summary>
        public static NameValueCollection RequestHeaders
        {
            get { return Context.Request.Headers; }
        }

        #endregion

        #region RequestHeaderValue

        /// <summary>
        /// REQUEST HEADERS 특정 KEY 의 VALUE 를 반환
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RequestHeaderValue(string key)
        {
            return RequestHeaders[key] ?? string.Empty;
        }

        #endregion

        #region RequestHeaderKeyValues

        /// <summary>
        /// REQUEST HEADERS 모든 KEY VALUE 반환
        /// </summary>
        public static Dictionary<string, string> RequestHeaderKeyValues
        {
            get { return !RequestHeaders.AllKeys.Any() ? null : RequestHeaders.AllKeys.ToDictionary(key => key, RequestHeaderValue); }
        }

        #endregion

        #region RequestQueryString

        /// <summary>
        /// REQUEST QUERYSTRING 반환
        /// </summary>
        public static NameValueCollection RequestQueryString
        {
            get { return Context.Request.QueryString; }
        }

        #endregion

        #region RequestQueryStringValue

        /// <summary>
        /// REQUEST QUERYSTRING 특정 KEY 의 VALUE 를 반환
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RequestQueryStringValue(string key)
        {
            return RequestQueryString[key] ?? string.Empty;
        }

        #endregion

        #region RequestQueryStringKeyValues

        /// <summary>
        /// REQUEST QUERYSTRING 모든 KEY VALUE 반환
        /// </summary>
        public static Dictionary<string, string> RequestQueryStringKeyValues
        {
            get { return !RequestQueryString.AllKeys.Any() ? null : RequestQueryString.AllKeys.ToDictionary(key => key, RequestQueryStringValue); }
        }

        #endregion

        #region RequestParams

        /// <summary>
        /// REQUEST PARAMS 반환
        /// </summary>
        public static NameValueCollection RequestParams
        {
            get { return Context.Request.Params; }
        }

        #endregion

        #region RequestParamsValue

        /// <summary>
        /// REQUEST PARAMS 특정 KEY 의 VALUE 를 반환
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RequestParamValue(string key)
        {
            return RequestParams[key] ?? string.Empty;
        }

        #endregion

        #region RequestParamsKeyValues

        /// <summary>
        /// REQUEST PARAMS 모든 KEY VALUE 반환
        /// </summary>
        public static Dictionary<string, string> RequestParamKeyValues
        {
            get { return !RequestParams.AllKeys.Any() ? null : RequestParams.AllKeys.ToDictionary(key => key, RequestParamValue); }
        }

        #endregion

        #region RequestForms

        /// <summary>
        /// REQUEST FORM 반환
        /// </summary>
        public static NameValueCollection RequestForm
        {
            get { return Context.Request.Form; }
        }

        #endregion

        #region RequestFormValue

        /// <summary>
        /// REQUEST FORM 특정 KEY 의 VALUE 를 반환
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RequestFormValue(string key)
        {
            return RequestForm[key] ?? string.Empty;
        }

        #endregion

        #region RequestFormKeyValues

        /// <summary>
        /// REQUEST FORM 모든 KEY VALUE 반환
        /// </summary>
        public static Dictionary<string, string> RequestFormKeyValues
        {
            get { return !RequestForm.AllKeys.Any() ? null : RequestForm.AllKeys.ToDictionary(key => key, RequestFormValue); }
        }

        #endregion

        #region RequestFile

        /// <summary>
        /// REQUEST FILES 반환
        /// </summary>
        public static HttpFileCollection RequestFiles
        {
            get { return Context.Request.Files; }
        }

        #endregion

        #region RequestBody

        /// <summary>
        /// REQUEST INPUTSTREAM 반환
        /// </summary>
        public static string RequestBody
        {
            get
            {
                string bodyString;
                using (
                    var readStream = new StreamReader(Context.Request.InputStream,
                        Encoding.GetEncoding("utf-8")))
                {
                    bodyString = readStream.ReadToEnd();
                }
                return bodyString;
            }
        }

        #endregion

        #region ClientIP

        /// <summary>
        /// REQUEST CLIENT 의 IPADDRESS 정보 반환
        /// </summary>
        public static string ClientIp
        {
            get { return Context.Request.UserHostAddress ?? string.Empty; }
        }

        #endregion

        #region RealClientIP

        /// <summary>
        /// REQUEST CLIENT 의 HTTP_X_FORWARDED_FOR 정보 반환
        /// </summary>
        public static string RealClientIp
        {
            get { return Context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? string.Empty; }
        }

        #endregion

        #region UrlHost

        /// <summary>
        /// REQUEST HOST 정보 반환
        /// </summary>
        public static string UrlHost
        {
            get { return Context.Request.Url.Host.ToLower(); }
        }

        #endregion

        #region UrlAbsolute

        /// <summary>
        /// REQUEST ABSOLUTEURI 정보 반환
        /// </summary>
        public static string UrlAbsolute
        {
            get { return Context.Request.Url.AbsoluteUri.ToLower(); }
        }

        #endregion

        #region UrlPath

        /// <summary>
        /// REQUEST ABSOLUTEPATH 정보 반환
        /// </summary>
        public static string UrlPath
        {
            get { return Context.Request.Url.AbsolutePath.ToLower(); }
        }

        #endregion

        #region UrlPort

        /// <summary>
        /// REQUEST PORT 정보 반환
        /// </summary>
        public static int UrlPort
        {
            get { return Context.Request.Url.Port; }
        }

        #endregion

        #region [Download Image]
        public static bool DownloadRemoteImageFile(string uri, string fileName)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            bool bImage = response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase);
            if ((response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Moved ||
                response.StatusCode == HttpStatusCode.Redirect) &&
                bImage)
            {
                using (Stream inputStream = response.GetResponseStream())
                using (Stream outputStream = File.OpenWrite(fileName))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    do
                    {
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        outputStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}