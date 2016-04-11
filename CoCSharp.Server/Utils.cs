using System;

namespace CoCSharp.Server
{
    public class Utils
    {
        public static Random Random = new Random();

        public static string BytesToString(byte[] bytes)
        {
            var str = string.Empty;
            for (int i = 0; i < bytes.Length; i++)
                str += bytes[i].ToString("x2");
            return str;
        }
    }
}
