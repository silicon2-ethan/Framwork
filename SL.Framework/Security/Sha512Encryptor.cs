using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SL.Framework.Security
{
    /// <summary>
    /// SHA512 암호화 클래스
    /// </summary>
    public class Sha512Encryptor : IEncryptor
    {
        private readonly Encoding _encoding;
        private readonly Func<byte[], string> _convertDelegate;

        public Sha512Encryptor(Encoding encoding, Func<byte[], string> convertDelegate)
        {
            this._encoding = encoding;
            this._convertDelegate = convertDelegate;
        }

        public Sha512Encryptor(Encoding encoding) : this(encoding, GetHexaDecimalString)
        {
        }

        public Sha512Encryptor() : this(Encoding.UTF8, GetHexaDecimalString)
        {
        }

        private static string GetHexaDecimalString(byte[] data)
        {
            var builder = new StringBuilder();
            foreach (var b in data)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }

        #region IEncryptor Members

        /// <summary>
        /// 이상함 사용하지 말것
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Encrypt(string value)
        {
            using (var provider = new SHA512CryptoServiceProvider())
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream, _encoding))
            {
                writer.Write(value);
                writer.Flush();
                var data = provider.ComputeHash(memoryStream);
                var data2 = provider.TransformFinalBlock(data, 0, data.Length);

                return _convertDelegate(data2);
            }
        }

        /// <summary>
        /// 실제 암호화 방식
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Encrypt2(string value)
        {
            using (var provider = new SHA512CryptoServiceProvider())
            {
                var enc = _encoding ?? Encoding.UTF8;
                var bytes = provider.ComputeHash(enc.GetBytes(value));
                var builder = new StringBuilder();
                foreach (var t in bytes)
                {
                    builder.Append(t.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string Decrypt(string value)
        {
            throw new CryptographicException("hash 알고리즘은 복고화할 수 없습니다.");
        }

        #endregion
    }
}