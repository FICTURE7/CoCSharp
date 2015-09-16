using System;

namespace CoCSharp.Logging
{
    public class ExceptionLogger : ILogger
    {
        public ExceptionLogger()
        {

        }

        public bool Active { get; set; }
        public string FileLog { get; set; }

        public void Log(LogCategory logCategory, params object[] paramters)
        {

        }
    }
}
