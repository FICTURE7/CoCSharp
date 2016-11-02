namespace CoCSharp.Server.API.Logging
{
    /// <summary>
    /// Represents an empty logger.
    /// </summary>
    public class EmptyLogger : Logger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyLogger"/> class.
        /// </summary>
        public EmptyLogger()
        {
            // Space
        }

        /// <summary/>
        protected override LogLevel Level => LogLevel.All;

        /// <summary/>
        protected override void Write(string message, LogLevel level)
        {
            // Space
        }
    }
}
