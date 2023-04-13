using NeoSmart.Utils;

namespace SL.Framework.Utility
{
    public class EncodeHelper
    {
        public static string Base64Encode(byte[] value)
        {
            return UrlBase64.Encode(value);
        }

        public static byte[] Base64Decode(string value)
        {
            return UrlBase64.Decode(value);
        }
    }
}