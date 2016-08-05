using System;

namespace CoCSharp.Server
{
    public static class Log
    {
        // Gets or sets a value indicating whether it should
        // log the full exception.
        public static bool FullException { get; set; }

        public static void Exception(string message, Exception ex)
        {
            var exMessage = FullException ? ex.ToString() + Environment.NewLine : ex.Message;
            Console.WriteLine("exception: {0}: {1}", message, exMessage);
        }

        public static void Warning(string message)
        {
            Console.WriteLine("warning: {0}", message);
        }

        public static void Info(string category, string message)
        {
            Console.WriteLine("{0}: {1}", category, message);
        }
    }
}
