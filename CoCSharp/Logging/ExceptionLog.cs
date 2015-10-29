using System;

namespace CoCSharp.Logging
{
    /// <summary>
    /// Represents a <see cref="Log"/> that logs <see cref="Exception"/>.
    /// </summary>
    public class ExceptionLog : Log
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionLog"/> class
        /// with the specified log name.
        /// </summary>
        /// <param name="logName">Name of log. The log name will be used as the <see cref="Log.Path"/>.</param>
        /// <exception cref="ArgumentNullException"/>
        public ExceptionLog(string logName) 
            : base(logName)
        {
            LogBuilder.Flags = LoggingFlags.Console | LoggingFlags.Properties;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionLog"/> class
        /// with the specified log name and path.
        /// </summary>
        /// <param name="logName">Name of log.</param>
        /// <param name="logPath">Path of log.</param>
        /// <exception cref="ArgumentNullException"/>
        public ExceptionLog(string logName, string logPath)
            : base(logName, logPath)
        {
            LogBuilder.Flags = LoggingFlags.Console | LoggingFlags.Properties;
        }

        /// <summary>
        /// Logs data with the specified parameters in this
        /// signature(Exception ex).
        /// </summary>
        /// <param name="parameters">The parameters needed to log the data.</param>
        public override void LogData(params object[] parameters)
        {
            if (!(parameters[0] is Exception))
                throw new ArgumentException("parameters[0] must be a type of an Exception.");

            var ex = (Exception)parameters[0];
            var exType = ex.GetType();

            var blockName = exType.Name;
            LogBuilder.OpenBlock(blockName);
            LogBuilder.AppendObject(ex);
            LogBuilder.CloseBlock();

            if (AutoSave) Save();
        }
    }
}
