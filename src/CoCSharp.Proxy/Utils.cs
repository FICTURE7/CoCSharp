using System;

namespace CoCSharp.Server
{
    public static class Utils
    {
        public static string BytesToHex(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            var str = string.Empty;
            for (int i = 0; i < bytes.Length; i++)
                str += bytes[i].ToString("x2");
            return str;
        }
    }
}
