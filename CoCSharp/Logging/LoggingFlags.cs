using System;

namespace CoCSharp.Logging
{
    /// <summary>
    /// Controls the <see cref="LogBuilder"/> on how it logs objects.
    /// </summary>
    [Flags]
    public enum LoggingFlags
    {
        /// <summary>
        /// Instructs the <see cref="LogBuilder"/> to log unknowns.
        /// </summary>
        Unknowns = 2,

        /// <summary>
        /// Instructs the <see cref="LogBuilder"/> to log properties.
        /// </summary>
        Properties = 4,

        /// <summary>
        /// Instructs the <see cref="LogBuilder"/> to log fields.
        /// </summary>
        Fields = 8
    };
}
