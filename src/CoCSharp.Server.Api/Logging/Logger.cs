using System;
using System.IO;

namespace CoCSharp.Server.Api.Logging
{
    /// <summary>
    /// Represents the base logger class and a chain of loggers.
    /// </summary>
    public abstract class Logger : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        protected Logger()
        {
            _sync = new object();
            ToConsole = true;
        }
        #endregion

        #region Fields & Properties
        // Figure out if we're disposed.
        private bool _disposed;
        private readonly object _sync;

        /// <summary>
        /// Gets or sets whether the <see cref="Logger"/> should log to the console.
        /// </summary>
        public virtual bool ToConsole { get; set; }

        /// <summary>
        /// Gets the name of the <see cref="Logger"/>.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets or sets the path of this <see cref="Logger"/> instance.
        /// </summary>
        public string Path { get; set; }
        #endregion

        #region Method
        /// <summary>
        /// Logs the specified object.
        /// </summary>
        /// <param name="toLog">Object to log.</param>
        public void Log(object toLog)
        {
            var log = Write(toLog);

            lock (_sync)
            {
                if (ToConsole)
                    Console.WriteLine(log);

                var file = new FileStream(Path, FileMode.Append);
                using (var writer = new StreamWriter(file))
                    writer.WriteLine(log);
            }
        }

        /// <summary>
        /// Method that is going to get called when <see cref="Log(object)"/> gets called.
        /// </summary>
        /// <param name="toLog">Message to log.</param>
        protected abstract string Write(object toLog);

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="Logger"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged resources and optionally managed resources used by the current 
        /// instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="disposing">Releases managed resources if set to <c>true</c>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Space
            }

            _disposed = true;
        }

        #endregion
    }
}
