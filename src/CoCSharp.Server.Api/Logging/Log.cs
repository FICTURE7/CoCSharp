using System;

namespace CoCSharp.Server.Api.Logging
{
    /// <summary>
    /// Represents a log.
    /// </summary>
    public class Log : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        public Log()
        {
            _logger = new EmptyLogger();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class with the specified <see cref="Logger"/>
        /// as the main <see cref="Logger"/>.
        /// </summary>
        /// <param name="mainLogger">Main <see cref="Logger"/> to use.</param>
        public Log(Logger mainLogger)
        {
            if (mainLogger == null)
                throw new ArgumentNullException(nameof(mainLogger));

            _logger = mainLogger;
        }

        private bool _disposed;
        private readonly Logger _logger;

        /// <summary>
        /// Gets the beginning of the chain of <see cref="Logger"/>.
        /// </summary>
        public Logger MainLogger => _logger;

        /// <summary>
        /// Logs the specified message as an info message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public void Info(string message)
        {
            lock (_logger)
            {
                _logger.Log(message, LogLevel.Info);
            }
        }

        /// <summary>
        /// Logs the specified message as a warning message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public void Warn(string message)
        {
            lock (_logger)
            {
                _logger.Log(message, LogLevel.Warn);
            }
        }

        /// <summary>
        /// Logs the specified message as an error message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public void Error(string message)
        {
            lock (_logger)
            {
                _logger.Log(message, LogLevel.Error);
            }
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="Log"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged resources and optionally managed resources used by the current 
        /// instance of the <see cref="Log"/> class.
        /// </summary>
        /// <param name="disposing">Releases managed resources if set to <c>true</c>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _logger.Dispose();
            }
            _disposed = true;
        }
    }
}
