using System.Collections.Generic;

namespace CoCSharp.Server.API.Logging
{
    /// <summary>
    /// Represents a log.
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        public Log()
        {
            // Space
        }

        /// <summary>
        /// Gets or sets the chain of <see cref="Logging.Logger"/>.
        /// </summary>
        public Logger Logger { get; set; }

        public void Info(string message)
        {
            if (Logger != null)
                Logger.Log(message, LogLevel.Info);
        }

        public void Warn(string message)
        {
            if (Logger != null)
                Logger.Log(message, LogLevel.Warn);
        }

        public void Error(string message)
        {
            if (Logger != null)
                Logger.Log(message, LogLevel.Error);
        }
   }
}
