namespace CoCSharp.Proxy
{
    public static partial class FancyConsole
    {
        public static void Write(object value)
        {
            var jobs = Parse(value.ToString());
            for (int i = 0; i < jobs.Count; i++)
                jobs[i].Write();
        }

        public static void Write(string format, object arg0)
        {
            Write(format, args: new object[] { arg0 });
        }

        public static void Write(string format, object arg0, object arg1)
        {
            Write(format, args: new object[] { arg0, arg1 });
        }

        public static void Write(string format, object arg0, object arg1, object arg2)
        {
            Write(format, args: new object[] { arg0, arg1, arg2 });
        }

        public static void Write(string format, object arg0, object arg1, object arg2, object arg3)
        {
            Write(format, args: new object[] { arg0, arg1, arg2, arg3 });
        }

        public static void Write(string format, params object[] args)
        {
            var value = string.Format(format, args);
            var jobs = Parse(value.ToString());
            for (int i = 0; i < jobs.Count; i++)
                jobs[i].Write();
        }
    }
}
