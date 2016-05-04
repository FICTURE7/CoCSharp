namespace CoCSharp.Server
{
    public static class Utils
    {
        public static string BytesToString(byte[] bytes)
        {
            var str = string.Empty;
            for (int i = 0; i < bytes.Length; i++)
                str += bytes[i].ToString("x2");
            return str;
        }
    }
}
