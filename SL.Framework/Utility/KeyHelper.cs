using System;
using System.Security.Cryptography;
using System.Text;

namespace SL.Framework.Utility
{
    public class KeyHelper
    {
        /// <summary>
        /// GUID
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string Guid(string type)
        {
            var result = System.Guid.NewGuid().ToString(type.ToUpper());
            return result;
        }

        /// <summary>
        /// RNG UNIQUE KEY
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string RngUniqueKey(int size = 0)
        {
            size = (size == 0) ? 10 : size;
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var charArray = chars.ToCharArray();
            var data = new byte[1];
            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            var result = new StringBuilder(size);
            foreach (var b in data)
            {
                result.Append(charArray[b%(charArray.Length - 1)]);
            }
            return result.ToString();
        }

        /// <summary>
        /// RNG API KEY
        /// </summary>
        /// <returns></returns>
        public static string RngAlphaNumKey(int size = 0)
        {
            var result = string.Empty;
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var array = chars.ToCharArray();
            var seed = Environment.TickCount;
            var random = new Random(seed);
            size = (size == 0) ? 20 : size;
            for (var i = 0; i < size; i++)
            {
                var num = random.Next(0, array.Length - 1);
                result += array[num];
            }
            return result;
        }
    }
}