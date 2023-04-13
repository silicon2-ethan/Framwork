using System;
using System.Security.Cryptography;
using System.Text;

namespace SL.Framework.Security
{
    /// <summary>
    /// DES 암호화 클래스.
    /// </summary>
    public class DesEncryptor : IEncryptor
    {
        private readonly Encoding _encoding;
        private readonly string _encrytionKey;

        public DesEncryptor(string key, Encoding encoding)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (encoding == null)
                throw new ArgumentNullException("encoding");

            _encrytionKey = key;
            _encoding = encoding;
        }

        #region IEncryptor Members

        public string Encrypt(string value)
        {
            string result;
            var valueArray = _encoding.GetBytes(value);
            var hashProvider = new MD5CryptoServiceProvider();
            var tripleDes = new TripleDESCryptoServiceProvider();
            try
            {
                var tdesKey = hashProvider.ComputeHash(_encoding.GetBytes(_encrytionKey));
                tripleDes.Key = tdesKey;
                tripleDes.Mode = CipherMode.ECB;
                tripleDes.Padding = PaddingMode.PKCS7;
                var cTransform = tripleDes.CreateEncryptor();

                var resultArray = cTransform.TransformFinalBlock(valueArray, 0, valueArray.Length);
                result = Convert.ToBase64String(resultArray);
            }
            finally
            {
                tripleDes.Clear();
                hashProvider.Clear();
            }
            return result;
        }

        public byte[] EncryptBytes(string value)
        {
            byte[] result;
            var valueArray = _encoding.GetBytes(value);
            var hashProvider = new MD5CryptoServiceProvider();
            var tripleDes = new TripleDESCryptoServiceProvider();
            try
            {
                var tdesKey = hashProvider.ComputeHash(_encoding.GetBytes(_encrytionKey));
                tripleDes.Key = tdesKey;
                tripleDes.Mode = CipherMode.ECB;
                tripleDes.Padding = PaddingMode.PKCS7;
                var cTransform = tripleDes.CreateEncryptor();

                result = cTransform.TransformFinalBlock(valueArray, 0, valueArray.Length);
            }
            finally
            {
                tripleDes.Clear();
                hashProvider.Clear();
            }
            return result;
        }

        public string Decrypt(string value)
        {
            string result;
            var valueArray = Convert.FromBase64String(value);
            var hashProvider = new MD5CryptoServiceProvider();
            var tripleDes = new TripleDESCryptoServiceProvider();
            try
            {
                var tdesKey = hashProvider.ComputeHash(_encoding.GetBytes(_encrytionKey));
                tripleDes.Key = tdesKey;
                tripleDes.Mode = CipherMode.ECB;
                tripleDes.Padding = PaddingMode.PKCS7;

                var cTransform = tripleDes.CreateDecryptor();
                var resultArray = cTransform.TransformFinalBlock(valueArray, 0, valueArray.Length);
                result = _encoding.GetString(resultArray);
            }
            finally
            {
                tripleDes.Clear();
                hashProvider.Clear();
            }
            return result;
        }

        public string DecryptBytes(byte[] value)
        {
            string result;
            var hashProvider = new MD5CryptoServiceProvider();
            var tripleDes = new TripleDESCryptoServiceProvider();
            try
            {
                var tdesKey = hashProvider.ComputeHash(_encoding.GetBytes(_encrytionKey));
                tripleDes.Key = tdesKey;
                tripleDes.Mode = CipherMode.ECB;
                tripleDes.Padding = PaddingMode.PKCS7;

                var cTransform = tripleDes.CreateDecryptor();
                var resultArray = cTransform.TransformFinalBlock(value, 0, value.Length);
                result = _encoding.GetString(resultArray);
            }
            finally
            {
                tripleDes.Clear();
                hashProvider.Clear();
            }
            return result;
        }

        #endregion
    }
}