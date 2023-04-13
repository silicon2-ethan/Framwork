
namespace SL.Framework.Security
{
    /// <summary>
    /// 기본 암호화 모듈 Interface.
    /// </summary>
    public interface IEncryptor
    {
        /// <summary>
        /// 암호화 한다.
        /// </summary>
        /// <param name="value">원본 문자열.</param>
        /// <returns>암호화된 문자열.</returns>
        string Encrypt(string value);

        /// <summary>
        /// 복호화 한다.
        /// </summary>
        /// <param name="value">복호화할 문자열.</param>
        /// <returns>원본 문자열.</returns>
        string Decrypt(string value);
    }
}