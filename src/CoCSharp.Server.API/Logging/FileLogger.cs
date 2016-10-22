using System.IO;

namespace CoCSharp.Server.API.Logging
{
    /// <summary>
    /// Represents a file logger.
    /// </summary>
    public class FileLogger : Logger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogger"/> class.
        /// </summary>
        public FileLogger()
        {
            _fileName = "filelogger.txt";
        }

        private readonly string _fileName;

        /// <summary/>
        protected override void WriteLog(string message, params object[] args)
        {
            using (var file = new StreamWriter(_fileName, true))
            {
                file.WriteLine(message, args);
            }
        }
    }
}
