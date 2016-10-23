namespace CoCSharp.Server.API.Logging
{
    /// <summary>
    /// Represents the base logger class and a chain of loggers.
    /// </summary>
    public abstract class Logger
    {
        #region Fields & Properties
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
        #endregion
    }
}
