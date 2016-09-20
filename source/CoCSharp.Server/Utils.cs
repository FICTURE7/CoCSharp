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

        internal static string GetDirectoryToken(string directory)
        {
            var seperator = directory.IndexOf("-") + 1;
            if (seperator == -1)
                return null;

            var token = directory.Substring(seperator, directory.Length - seperator);
            return token;
        }

        internal static string GetDirectoryUserID(string directory)
        {
            var seperator = directory.IndexOf("-");
            if (seperator == -1)
                return null;

            var userIDStr = directory.Substring(0, seperator);
            return userIDStr;
        }
    }
}
