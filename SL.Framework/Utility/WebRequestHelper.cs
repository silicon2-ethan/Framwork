using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace SL.Framework.Utility
{
    /// <summary>
    /// WebRequestHelper
    /// </summary>
    public class WebRequestHelper
    {
        static WebRequestHelper()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
        }

        public static string GetParameter(Dictionary<string, string> parameters)
        {
            var result = string.Empty;
            foreach (var item in parameters)
            {
                if (string.IsNullOrEmpty(item.Key)) continue;
                if (string.IsNullOrEmpty(result))
                {
                    result = item.Key + "=" + HttpUtility.UrlEncode(item.Value);
                }
                else
                {
                    result += "&" + item.Key + "=" + HttpUtility.UrlEncode(item.Value);
                }
            }
            return result;
        }

        /// <summary>
        /// GetRequest
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static HttpWebRequest GetRequest(WebRequestHelperModel model)
        {
            try
            {
                if (model.UnsafeAuthenticatedConnectionSharing == true) //해당 서버에 https 인증서가 설치되어 있으므로.
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3 | (SecurityProtocolType)3072;
                }

                var request = (HttpWebRequest)WebRequest.Create(model.Url);

                request.Method = model.Method;
                request.Timeout = model.Timeout;
                request.ReadWriteTimeout = model.ReadWriteTimeout;
                request.Accept = model.Accept;
                request.ContentType = model.ContentType;
                request.UserAgent = model.UserAgent;
                request.Referer = model.Referer;
                request.KeepAlive = model.KeepAlive;
                request.AllowAutoRedirect = model.AllowAutoRedirect;
                request.UnsafeAuthenticatedConnectionSharing = model.UnsafeAuthenticatedConnectionSharing;
                request.PreAuthenticate = model.PreAuthenticate;
                request.CookieContainer = model.CookieContainer;
                request.Credentials = model.Credential;

                // HEADER SETTING
                if (model.Headers != null && model.Headers.Count > 0)
                {
                    foreach (var header in model.Headers)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }

                // POST SETTING
                if (model.Method.Equals("POST", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(model.FormData))
                {
                    request.ContentLength = 0;

                    var bytes = model.FormDataEnc.GetBytes(model.FormData);
                    request.ContentLength = bytes.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
                else if (model.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(model.FormData))
                {
                    request.ContentLength = 0;

                    var bytes = model.FormDataEnc.GetBytes(model.FormData);
                    request.ContentLength = bytes.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
                else if (model.Method.Equals("DELETE", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(model.FormData))
                {
                    request.ContentLength = 0;

                    var bytes = model.FormDataEnc.GetBytes(model.FormData);
                    request.ContentLength = bytes.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception("[GETREUQEST] - " + ex.Message);
            }
        }

        /// <summary>
        /// GetResponse
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static HttpWebResponse GetResponse(WebRequestHelperModel model)
        {
            try
            {
                return (HttpWebResponse)GetRequest(model).GetResponse();
            }
            catch (WebException ex)
            {
                throw new Exception("[GETRESPONSE] - " + ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("[GETREUQEST]"))
                    throw;

                throw new Exception("[GETRESPONSE] - " + ex.Message);
            }
        }

        /// <summary>
        /// GetStream
        /// </summary>
        public static Stream GetStream(WebRequestHelperModel model)
        {
            try
            {
                using (var response = GetResponse(model))
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream == null) return null;
                        var mem = new MemoryStream();
                        stream.CopyTo(mem);
                        mem.Seek(0, SeekOrigin.Begin);
                        return mem;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("[GETRESPONSE]"))
                    throw;

                throw new Exception("[GETSTREAM] - " + ex.Message);
            }
        }

        /// <summary>
        /// GetString
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetString(WebRequestHelperModel model)
        {
            try
            {
                using (var stream = GetStream(model))
                {
                    using (var reader = new StreamReader(stream, model.ReaderEnc))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("[GETRESPONSE]"))
                    throw;

                if (ex.Message.Contains("[GETSTREAM]"))
                    throw;

                throw new Exception("[GETSTRING] - " + ex.Message);
            }
        }
    }

    /// <summary>
    /// WebRequestHelperModel
    /// </summary>
    public class WebRequestHelperModel
    {
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Method [GET / POST]
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Timeout
        /// Default : 100000
        /// </summary>
        public int Timeout { get; set; } = 100000;

        /// <summary>
        /// ReadWriteTimeout
        /// Default : 300000
        /// </summary>
        public int ReadWriteTimeout { get; set; } = 300000;

        /// <summary>
        /// Accept
        /// Default : null
        /// </summary>
        public string Accept { get; set; } = null;

        /// <summary>
        /// UserAgent
        /// Default : null
        /// </summary>
        public string UserAgent { get; set; } = null;

        /// <summary>
        /// ContentType
        /// Default : application/x-www-form-urlencoded
        /// </summary>
        public string ContentType { get; set; } = "application/x-www-form-urlencoded";

        /// <summary>
        /// Referer
        /// Default : null
        /// </summary>
        public string Referer { get; set; } = null;

        /// <summary>
        /// KeepAlive
        /// Default : true
        /// </summary>
        public bool KeepAlive { get; set; } = true;

        /// <summary>
        /// AllowAutoRedirect
        /// Default : true
        /// </summary>
        public bool AllowAutoRedirect { get; set; } = true;

        /// <summary>
        /// PreAuthenticate
        /// Default : false
        /// </summary>
        public bool PreAuthenticate { get; set; } = false;

        /// <summary>
        /// UnsafeAuthenticatedConnectionSharing
        /// Default : false
        /// </summary>
        public bool UnsafeAuthenticatedConnectionSharing { get; set; } = false;

        /// <summary>
        /// CookieContainer
        /// Default : null
        /// </summary>
        public CookieContainer CookieContainer { get; set; } = null;

        /// <summary>
        /// Headers
        /// Default : null
        /// </summary>
        public IDictionary<string, string> Headers { get; set; } = null;

        /// <summary>
        /// FormData
        /// Default : null
        /// </summary>
        public string FormData { get; set; } = null;

        /// <summary>
        /// FormDataEnc
        /// Default : UTF8
        /// </summary>
        public Encoding FormDataEnc { get; set; } = Encoding.UTF8;

        /// <summary>
        /// ReaderEnd
        /// Default : UTF8
        /// </summary>
        public Encoding ReaderEnc { get; set; } = Encoding.UTF8;

        /// <summary>
        /// Credential (APIKey, P/W 적용시 필요)
        /// Default : null
        /// </summary>
        public NetworkCredential Credential { get; set; } = null;
    }
}