using System;

namespace CoCSharp.Server.API.Logging
{
    /// <summary>
    /// Represents the level of the log.
    /// </summary>
    [Flags]
    public enum LogLevel
    {
        /// <summary>
        /// Nothing.
        /// </summary>
        None = 0,

        /// <summary>
        /// Debug log.
        /// </summary>
        Debug = 1,

        /// <summary>
        /// Informative log.
        /// </summary>
        Info = 2,

        /// <summary>
        /// Warning log.
        /// </summary>
        Warn = 4,

        /// <summary>
        /// Error log.
        /// </summary>
        Error = 8,

        /// <summary>
        /// All logs.
        /// </summary>
        All = Debug | Info | Warn | Error
    }
}
