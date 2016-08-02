using System;

namespace CoCSharp.Server.Core
{
    public static partial class FancyConsole
    {
        public static void WriteLine()
        {
            if (!Enabled)
                return;

            Console.WriteLine();
        }

        public static void WriteLine(object value)
        {
            if (!Enabled)
                return;

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
            if (!Enabled)
                return;

            Write(format, args);
            WriteLine();
        }
    }
}
