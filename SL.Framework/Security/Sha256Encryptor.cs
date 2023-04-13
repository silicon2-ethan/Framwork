using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SL.Framework.Security
{
    /// <summary>
    ///  SHA256 암호화 모듈
    /// </summary>
    public class Sha256Encryptor : IEncryptor
    {
        private readonly Encoding _encoding;
        private readonly Func<byte[], string> _convertDelegate;

        public Sha256Encryptor(Encoding encoding, Func<byte[], string> convertDelegate)
        {
            this._encoding = encoding;
            this._convertDelegate = convertDelegate;
        }

        public Sha256Encryptor(Encoding encoding) : this(encoding, GetHexaDecimalString)
        {
        }

        public Sha256Encryptor() : this(Encoding.UTF8, GetHexaDecimalString)
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

        public string Decrypt(string value)
        {
            throw new CryptographicException("hash 알고리즘은 복고화할 수 없습니다.");
        }

        public string Encrypt(string value)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var enc = _encoding ?? Encoding.UTF8;
                var bytes = sha256.ComputeHash(enc.GetBytes(value));
                var builder = new StringBuilder();
                foreach (var t in bytes)
                {
                    builder.Append(t.ToString("x2"));
                }
                return builder.ToString();
            }

            /*QLPS Admin과 암호화하는 방식이 달라서 주석처리*/
            //using (var provider = new SHA256CryptoServiceProvider())
            //{
            //    using (var memoryStream = new MemoryStream())
            //    {
            //        using (var writer = new StreamWriter(memoryStream, _encoding))
            //        {
            //            writer.Write(value);
            //            writer.Flush();
            //            var data = provider.ComputeHash(memoryStream);
            //            var data2 = provider.TransformFinalBlock(data, 0, data.Length);
            //            return _convertDelegate(data2);
            //        }
            //    }
            //}
        }
    }
}