using System;
using System.Security.Cryptography;
using System.Text;

namespace SL.Framework.Security
{
    /// <summary>
    /// SHA1 암호화 클래스
    /// </summary>
    public class Sha1Encryptor
    {
        private readonly Encoding _encoding;
        private readonly Func<byte[], string> _convertDelegate;

        /// <summary>
        /// SHA1 암호화 클래스를 초기화한다.
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="convertDelegate"></param>
        public Sha1Encryptor(Encoding encoding, Func<byte[], string> convertDelegate)
        {
            this._encoding = encoding;
            this._convertDelegate = convertDelegate;
        }

        /// <summary>
        /// SHA1 암호화 클래스를 초기화한다.
        /// </summary>
        /// <param name="encoding"></param>
        public Sha1Encryptor(Encoding encoding) : this(encoding, GetHexaDecimalString)
        {
        }

        /// <summary>
        /// SHA1 암호화 클래스를 초기화한다.
        /// </summary>
        public Sha1Encryptor() : this(Encoding.UTF8, GetHexaDecimalString)
        {
        }

        /// <summary>
        /// Hexa 타입 문자열로 변환한다.
        /// </summary>
        /// <param name="data">배열 바이트 문자열.</param>
        /// <returns>Hexa 문자열.</returns>
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
        /// 문자열을 SHA1 알고리즘으로 암호화 한다.
        /// </summary>
        /// <param name="value">문자열.</param>
        /// <returns>암호화된 문자열</returns>
        public string Encrypt(string value)
        {
            var sha1 = SHA1.Create();

            if (sha1 == null)
                throw new CryptographicException("SHA1 algorithm instance creation failed.");

            var sha1Hash = sha1.ComputeHash(_encoding.GetBytes(value));
            return _convertDelegate(sha1Hash);
        }

        /// <summary>
        /// 암호화된 문자열을 복호화 한다. (SHA1 알고리즘은 복호화가 안됨)
        /// </summary>
        /// <param name="value">암호화된 문자열.</param>
        /// <returns>복호화된 문자열.</returns>
        public string Decrypt(string value)
        {
            throw new CryptographicException("hash 알고리즘은 복고화할 수 없습니다.");
        }

        #endregion
    }
}