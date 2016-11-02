using System;

namespace CoCSharp.Server.API.Logging
{
    /// <summary>
    /// Represents the base logger class and a chain of loggers.
    /// </summary>
    public abstract class Logger : IDisposable
    {
        #region Fields & Properties
        private bool _disposed;
        private Logger _nextLogger;

        /// <summary>
        /// Gets the next <see cref="Logger"/> in the responsibility chain.
        /// </summary>
        public Logger NextLogger => _nextLogger;

        /// <summary>
        /// Gets the <see cref="LogLevel"/> of the <see cref="Logger"/>.
        /// </summary>
        protected abstract LogLevel Level { get; }
        #endregion

        #region Method
        /// <summary>
        /// Sets the next <see cref="Logger"/> in the responsibility chain.
        /// </summary>
        /// <param name="logger"><see cref="Logger"/> to be next in the responsibility chain.</param>
        /// <returns>Same instance as <paramref name="logger"/>.</returns>
        public Logger Next(Logger logger)
        {
            // Avoid circular references to avoid recursion and a StackOverflowException.
            if (logger == this)
                throw new ArgumentException("Logger must not be of the same instance.");

            _nextLogger = logger;
            return logger;
        }

        /// <summary>
        /// Logs the specified message with the specified arguments and passes those
        /// parameters to the <see cref="NextLogger"/> if not null.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="level">Level of the log.</param>
        public void Log(string message, LogLevel level)
        {
            if (Level.HasFlag(level))
                Write(message, level);

            if (_nextLogger != null)
                _nextLogger.Log(message, level);
        }

        /// <summary>
        /// Method that is going to get called when <see cref="Log(string, LogLevel)"/> is called.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="level">Level of the log.</param>
        protected abstract void Write(string message, LogLevel level);

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="Logger"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged resources and optionally managed resources used by the current 
        /// instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="disposing">Releases managed resources if set to <c>true</c>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Dispose the logger who is next in the chain.
                _nextLogger?.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
