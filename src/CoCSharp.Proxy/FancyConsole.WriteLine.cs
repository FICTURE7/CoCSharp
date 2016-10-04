using System;

namespace CoCSharp.Proxy
{
    public static partial class FancyConsole
    {
        public static void WriteLine()
        {
            Console.WriteLine();
        }

        public static void WriteLine(object value)
        {
            Write(value);
            WriteLine();
        }

        public static void WriteLine(string format, object arg0)
        {
            WriteLine(format, args: new object[] { arg0 });
        }

        public static void WriteLine(string format, object arg0, object arg1)
        {
            WriteLine(format, args: new object[] { arg0, arg1 });
        }

        public static void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            WriteLine(format, args: new object[] { arg0, arg1, arg2 });
        }

        public static void WriteLine(string format, object arg0, object arg1, object arg2, object arg3)
        {
            WriteLine(format, args: new object[] { arg0, arg1, arg2, arg3 });
        }

        public static void WriteLine(string format, params object[] args)
        {
            Write(format, args);
            WriteLine();
        }
    }
}
