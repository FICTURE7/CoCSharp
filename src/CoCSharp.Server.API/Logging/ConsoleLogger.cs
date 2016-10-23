using System;

namespace CoCSharp.Server.API.Logging
{
    /// <summary>
    /// Represents a <see cref="Logger"/> that logs to the <see cref="Console"/>.
    /// </summary>
    public class ConsoleLogger : Logger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleLogger"/> class.
        /// </summary>
        public ConsoleLogger()
        {
            // Space
        }

        /// <summary/>
        protected override LogLevel Level => LogLevel.All;

        /// <summary/>
        protected override void Write(string message, LogLevel level)
        {
            Console.WriteLine("[" + level + "] -> " + message);
        }
    }
}
