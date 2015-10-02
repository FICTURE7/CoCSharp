using System;
using System.IO;
using System.Text;

namespace CoCSharp.Logging
{
    public class ExceptionLogger
    {
        public ExceptionLogger()
        {
            LogConsole = true;
            LogWriter = new StreamWriter("exceptions.log", true)
            {
                AutoFlush = true
            };
        }

        public bool LogConsole { get; set; }

        private StreamWriter LogWriter { get; set; }

        public void LogException(Exception ex)
        {
            var builder = new StringBuilder();
            var exType = ex.GetType();

            builder.Append(DateTime.Now.ToString("[~HH:mm:ss.fff] "));
            builder.Append(exType.Name);
            builder.AppendLine();
            builder.AppendLine("{");

            builder.AppendFormat("    Message: {0}\r\n", ex.Message);
            builder.AppendLine("    StackTrace:");
            builder.AppendLine("    {");
            builder.Append("     " + ex.StackTrace.Replace("\r\n", "\r\n     ")); // fix indentation
            builder.AppendLine("\r\n    }");

            builder.AppendFormat("    TargetSite: {0}\r\n", ex.TargetSite);
            builder.AppendLine("}");
            
            var builderString = builder.ToString();

            LogWriter.WriteLine(builderString);
            if (LogConsole)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("Exception");
                Console.ResetColor();
                Console.Write("] ");
                Console.WriteLine(builderString);
            }
        }
    }
}
