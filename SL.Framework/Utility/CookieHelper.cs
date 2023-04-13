using System;
using System.Web;

namespace SL.Framework.Utility
{
    #region [Basic Cookie Helper]
    /// <summary>
    /// COOKIE 도우미
    /// </summary>
    public class CookieHelper
    {
        /// <summary>
        /// HTTP CONTEXT
        /// </summary>
        private static HttpContext Context
        {
            get { return HttpContext.Current; }
        }

        /// <summary>
        /// SET COOKIE
        /// </summary>
        /// <param name="cookie"></param>
        public static void Set(HttpCookie cookie)
        {
            cookie.Path = "/";
            cookie.Domain = Context.Request.Url.Host.Substring(Context.Request.Url.Host.IndexOf('.'));
            Context.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// SET COOKIE
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isHttp"></param>
        /// <param name="expireDays"></param>
        public static void Set(string key, string value, bool isHttp, int expireDays)
        {
            var cookie = new HttpCookie(key)
            {
                Value = value,
                Expires = DateTime.Now.AddDays(expireDays),
                HttpOnly = isHttp
            };

            Context.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// GET COOKIE
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static HttpCookie Get(string key)
        {
            var cookie = Context.Request.Cookies.Get(key);
            if (cookie != null)
            {
                cookie.Domain = Context.Request.Url.Host.Substring(Context.Request.Url.Host.IndexOf('.'));
                if (cookie.Value.vIsEmpty()) return null;
                if (cookie.Value.Trim().Length <= 0) return null;
                return cookie;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// GET COOKIE VALUE
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            var cookie = Get(key);
            return cookie != null
                ? cookie.Value
                : null;
        }

        /// <summary>
        /// EXIST COOKIE
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            return Get(key) != null;
        }

        /// <summary>
        /// REMOVE COOKIE
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            Context.Request.Cookies.Remove(key);
        }

        /// <summary>
        /// DELETE COOKIE
        /// </summary>
        /// <param name="key"></param>
        public static void Delete(string key)
        {
            if (!Exists(key)) return;

            var cookie = new HttpCookie(key)
            {
                Expires = new DateTime(1900, 1, 1),
                Value = null,
                Path = "/",
                Domain = Context.Request.Url.Host.Substring(Context.Request.Url.Host.IndexOf('.'))
            };
            Context.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// DELETE COOKIE ALL
        /// </summary>
        /// <param name="deleteServerCookies">True to delete server cookies.
        /// The default is false.</param>
        public static void DeleteAll(bool deleteServerCookies = false)
        {
            for (var i = 0; i <= Context.Request.Cookies.Count - 1; i++)
            {
                if (Context.Request.Cookies[i] != null)
                {
                    Delete(Context.Request.Cookies[i].Name);
                }
            }

            if (deleteServerCookies)
            {
                Context.Request.Cookies.Clear();
            }
        }
    }
    #endregion




}