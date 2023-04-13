using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SL.Framework.Security
{
    /// <summary>
    /// AES 128 암호화 클래스.
    /// </summary>
    public class AES128Encryptor : IEncryptor
    {
        private readonly Encoding _encoding;
        private readonly RijndaelManaged _cipher;

        public AES128Encryptor(string key, Encoding encoding)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (encoding == null)
                throw new ArgumentNullException("encoding");

            var keyBytes = new byte[key.Length];
            Array.Copy(encoding.GetBytes(key), keyBytes, keyBytes.Length);

            _cipher = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                Key = keyBytes
            };

            _encoding = encoding;
        }

        #region IEncryptor Members

        public string Encrypt(string value)
        {
            _cipher.GenerateIV();
            var valueBytes = _encoding.GetBytes(value);
            var encryptor = _cipher.CreateEncryptor();
            var secureBytes = encryptor.TransformFinalBlock(valueBytes, 0, valueBytes.Length);
            secureBytes = _cipher.IV.Concat(secureBytes).ToArray();
            return Convert.ToBase64String(secureBytes);
        }

        public string Decrypt(string value)
        {
            var blockSize = _cipher.IV.Length;
            var secureBytes = Convert.FromBase64String(value);
            _cipher.IV = secureBytes.Take(blockSize).ToArray();
            var decryptor = _cipher.CreateDecryptor();
            var plainBytes = decryptor.TransformFinalBlock(secureBytes, blockSize, secureBytes.Length - blockSize);
            return _encoding.GetString(plainBytes);
        }

        #endregion
    }
}