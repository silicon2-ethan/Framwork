using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SL.Framework.Security
{
    /// <summary>
    /// RC2 암호화 클래스.
    /// </summary>
    public class Rc2Encryptor : IEncryptor
    {
        private readonly SymmetricAlgorithm _cryptoProvider = new RC2CryptoServiceProvider();
        private readonly string _encrytionKey;

        public Rc2Encryptor(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            _encrytionKey = key;
        }

        #region IEncryptor Members

        public string Encrypt(string value)
        {
            var keyBytes = Encoding.ASCII.GetBytes(_encrytionKey);
            using (var memoryStream = new MemoryStream())
            {
                var encryptor = _cryptoProvider.CreateEncryptor(keyBytes, keyBytes);
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                using (var writer = new StreamWriter(cryptoStream))
                {
                    writer.Write(value);
                    writer.Flush();
                    cryptoStream.FlushFinalBlock();
                    return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int) memoryStream.Length);
                }
            }
        }

        public string Decrypt(string value)
        {
            var keyBytes = Encoding.ASCII.GetBytes(_encrytionKey);
            using (var memoryStream = new MemoryStream(Convert.FromBase64String(value)))
            {
                var decryptor = _cryptoProvider.CreateDecryptor(keyBytes, keyBytes);
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cryptoStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        #endregion
    }
}