using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SL.Framework.Security;
using SL.Framework.Utility;

namespace SL.Framework.Extension
{
    /// <summary>
    /// AES 256 암호화
    /// </summary>
    public class AESEncapsulationExtension : IEncryptor
    {
        #region [Properties]
        /// <summary>
        /// 암/복호화 키 (절대로 바꾸지 마세요..)
        /// </summary>
        readonly string encKey = "silicon2!@#$%6789werwrwerwerczcvzxcvsdfsfwerslkdfjsldczz24234@@";

        string realKey
        {
            get
            {
                return encKey.Substring(0, 256 / 8);
            }
        }

        /// <summary>
        /// 웹에서사용할 경우 특수문자 치환
        /// </summary>
        Func<string, string> urlEncoding = delegate (string value)
        {
            return value.Replace("_g_", "_g_8_").Replace("+", "_g_1_").Replace("/", "_g_2_").Replace("=", "_g_3_").Replace("&", "_g_4_").Replace("<", "_g_5_").Replace(">", "_g_6_").Replace("@", "_g_7_");
        };

        /// <summary>
        /// 웹에서사용할 경우 특수문자 치환
        /// </summary>
        Func<string, string> urlDecoding = delegate (string value)
        {
            return value.Replace("_g_1_", "+").Replace("_g_2_", "/").Replace("_g_3_", "=").Replace("_g_4_", "&").Replace("_g_5_", "<").Replace("_g_6_", ">").Replace("_g_7_", "@").Replace("_g_8_", "_g_");
        };
        #endregion

        /// <summary>
        /// 복호화
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Decrypt(string value)
        {
            if (value.vIsEmpty()) return "";
            value = this.urlDecoding(value);

            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes(this.realKey);
            aes.IV = new byte[] { 0, 10, 30, 50, 70, 90, 15, 25, 35, 45, 55, 65, 75, 85, 95, 0 };

            var decrypt = aes.CreateDecryptor();
            byte[] xBuff = null;
            try
            {
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Convert.FromBase64String(value);
                        cs.Write(xXml, 0, xXml.Length);
                    }

                    xBuff = ms.ToArray();
                }

                String Output = Encoding.UTF8.GetString(xBuff);
                return Output;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 암호화
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Encrypt(string value)
        {
            if (value.vIsEmpty()) return "";
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes(this.realKey);
            aes.IV = new byte[] { 0, 10, 30, 50, 70, 90, 15, 25, 35, 45, 55, 65, 75, 85, 95, 0 };

            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            try
            {
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Encoding.UTF8.GetBytes(value);
                        cs.Write(xXml, 0, xXml.Length);
                    }

                    xBuff = ms.ToArray();
                }

                String Output = Convert.ToBase64String(xBuff);
                return this.urlEncoding(Output);
            }
            catch (Exception)
            {
                return string.Empty;
            }                       
        }
    }
}
