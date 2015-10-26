using System;
using System.IO;

namespace CoCSharp.Logging
{
    /// <summary>
    /// Provides methods to write log. 
    /// </summary>
    public abstract class Log
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class 
        /// with the specified log name.
        /// </summary>
        /// <param name="logName">Name of log. The log name will be used as the <see cref="Path"/>.</param>
        /// <exception cref="ArgumentNullException"/>
        public Log(string logName)
        {
            if (logName == null)
                throw new ArgumentNullException(logName);
            Name = logName;
            Path = logName + ".log";
            LogBuilder = new LogBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class
        /// with the specified log name and path.
        /// </summary>
        /// <param name="logName">Name of log.</param>
        /// <param name="path">Path of log.</param>
        /// <exception cref="ArgumentNullException"/>
        public Log(string logName, string path)
        {
            if (logName == null)
                throw new ArgumentNullException(logName);
            if (path == null)
                throw new ArgumentNullException(path);
            Name = logName;
            Path = path;
            LogBuilder = new LogBuilder();
        }

        /// <summary>
        /// Gets the name of the log.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the path of the log.
        /// </summary>
        public string Path { get; private set; }
        /// <summary>
        /// Gets or sets whether the <see cref="Log"/> is going to
        /// save everytime after <see cref="LogData(object[])"/> is called.
        /// </summary>
        public virtual bool AutoSave { get; set; }

        /// <summary>
        /// Gets or sets the log as a string.
        /// </summary>
        protected string LogString
        {
            set { LogBuilder = new LogBuilder(value); }
            get { return LogBuilder.ToString(); }
        }
        /// <summary>
        /// Gets or sets the <see cref="Logging.LogBuilder"/> used to
        /// build this log.
        /// </summary>
        protected LogBuilder LogBuilder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        public abstract void LogData(params object[] parameters);

        /// <summary>
        /// 
        /// </summary>
        public virtual void Save()
        {
            File.WriteAllText(Path, LogString);
        }

        /// <summary>
        /// Returns a string that represent the current <see cref="Log"/>.
        /// </summary>
        /// <returns>A string that represent the current <see cref="Log"/>.</returns>
        public override string ToString()
        {
            return LogString;
        }
    }
}
