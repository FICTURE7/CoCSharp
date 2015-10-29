using System;

namespace CoCSharp.Logging
{
    /// <summary>
    /// Instruct the <see cref="LogBuilder"/> to build the log of
    /// the field or property value adding a new layer of depth
    /// to the log.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class LoggableAttribute : Attribute
    {
        //TODO: Start using this bad boi.

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggableAttribute"/>
        /// class. <see cref="Flags"/> bit mask is going to be set to default.
        /// <code>LoggingFlags.Fields | LoggingFlags.Unknowns</code>.
        /// </summary>
        public LoggableAttribute()
        {
            Flags = LoggingFlags.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggableAttribute"/>
        /// class with the specified <see cref="LoggingFlags"/>.
        /// </summary>
        /// <param name="flags"></param>
        public LoggableAttribute(LoggingFlags flags)
        {
            Flags = flags;
        }

        /// <summary>
        /// Instruct the <see cref="LogBuilder"/> in the specified
        /// way. This is a bit mask.
        /// </summary>
        public LoggingFlags Flags { get; set; }
    }
}
