using System;
using System.IO;

namespace CoCSharp.Server.Api.Logging
{
    /// <summary>
    /// Represents a file logger.
    /// </summary>
    public class FileLogger : Logger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogger"/> class.
        /// </summary>
        public FileLogger(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var now = DateTime.Now.ToString("dd-MM-yy_hh-mm-ss");

            _fileName = Path.Combine("logs", now + ".log");
            _file = new FileStream(_fileName, FileMode.Append);
            _writer = new StreamWriter(_file);
        }

        private bool _disposed;
        private readonly string _fileName;
        private readonly FileStream _file;
        private readonly StreamWriter _writer;

        /// <summary/>
        protected override LogLevel Level => LogLevel.All;

        /// <summary/>
        protected override void Write(string message, LogLevel level)
        {
            _writer.WriteLine("[" + level + "] -> " + message);
            _writer.Flush();
        }

        /// <summary/>
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                //_file.Dispose();

                // StreamWriter will dispose the BaseStream.
                _writer.Dispose();
            }

            _disposed = true;
            base.Dispose(disposing);
        }
    }
}
