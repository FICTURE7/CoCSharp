using System;

namespace CoCSharp.Server.Api.Logging
{
    /// <summary>
    /// Represents a logger that logs 'info' level logs.
    /// </summary>
    public class WarnLogger : Logger
    {
        /// <summary>
        /// Gets the name of the <see cref="ErrorLogger"/>.
        /// </summary>
        public override string Name => "warn";

        /// <summary>
        /// Method that is going to get called when <see cref="Logger.Log(object)"/> gets called.
        /// </summary>
        /// <param name="toLog">Message to log.</param>
        protected override string Write(object toLog)
        {
            return $"[{DateTime.UtcNow.ToString("yy/MM/dd - hh:mm:ss.fff")}][warn] - {toLog}";
        }
    }
}
