using System;
using System.IO;

namespace CoCSharp.Server.API.Logging
{
    /// <summary>
    /// Represents a file logger.
    /// </summary>
    public class FileLogger : Logger
    {
        static FileLogger()
        {
            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogger"/> class.
        /// </summary>
        public FileLogger()
        {
            var now = DateTime.Now.ToString("dd-MM-yy_hh-mm-ss");
            _fileName = Path.Combine("logs", now + ".log");
        }

        private readonly string _fileName;

        /// <summary/>
        protected override LogLevel Level => LogLevel.All;

        /// <summary/>
        protected override void Write(string message, LogLevel level)
        {
            using (var file = new StreamWriter(_fileName, true))
            {
                file.WriteLine(message);
            }
        }
    }
}
