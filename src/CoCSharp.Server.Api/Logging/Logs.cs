using System;
using System.Collections.Generic;
using System.IO;

namespace CoCSharp.Server.Api.Logging
{
    /// <summary>
    /// Represents a group of logs.
    /// </summary>
    public class Logs : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Logs"/> class with the specified path.
        /// </summary>
        /// <param name="path">Root directory of logs.</param>
        public Logs(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            _sync = new object();
            _rootDir = path;
            _loggers = new Dictionary<Type, Logger>();

            RegisterLogger<InfoLogger>();
            RegisterLogger<WarnLogger>();
            RegisterLogger<ErrorLogger>();
        }
        #endregion

        #region Fields & Properties
        private bool _disposed;
        private readonly object _sync;
        private readonly string _rootDir;
        private readonly Dictionary<Type, Logger> _loggers;

        /// <summary>
        /// Gets the number of <see cref="Logger"/> instances attached to the group of logs.
        /// </summary>
        public int Count => _loggers.Count;
        #endregion

        #region Methods
        /// <summary>
        /// Returns the <see cref="Logger"/> instance of the specified type.
        /// </summary>
        /// <typeparam name="TLogger">Type of <see cref="Logger"/> to return.</typeparam>
        /// <returns>Returns a logger instance of the specified type; returns null if not attached.</returns>
        public TLogger GetLogger<TLogger>() where TLogger : Logger, new()
        {
            var logger = default(Logger);
            _loggers.TryGetValue(typeof(TLogger), out logger);
            return (TLogger)logger;
        }

        /// <summary>
        /// Adds the specified <see cref="Logger"/> type to this <see cref="Logs"/> instance.
        /// </summary>
        /// <typeparam name="TLogger">Type of <see cref="Logger"/> to add.</typeparam>
        public void RegisterLogger<TLogger>() where TLogger : Logger, new()
        {
            var type = typeof(TLogger);
            if (_loggers.ContainsKey(type))
                throw new Exception();

            var logger = new TLogger();
            logger.Path = Path.Combine(_rootDir, logger.Name + ".log");

            _loggers.Add(type, logger);
        }

        /// <summary>
        /// Logs the specified message as an info message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public void Info(string message)
        {
            lock (_sync)
            {
                GetLogger<InfoLogger>().Log(message);
            }
        }

        /// <summary>
        /// Logs the specified message as a warning message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public void Warn(string message)
        {
            lock (_sync)
            {
                GetLogger<WarnLogger>().Log(message);
            }
        }

        /// <summary>
        /// Logs the specified message as an error message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public void Error(string message)
        {
            lock (_sync)
            {
                GetLogger<ErrorLogger>().Log(message);
            }
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="Logs"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged resources and optionally managed resources used by the current 
        /// instance of the <see cref="Logs"/> class.
        /// </summary>
        /// <param name="disposing">Releases managed resources if set to <c>true</c>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                foreach (var vk in _loggers)
                {
                    var logger = vk.Value;
                    logger.Dispose();
                }
            }
            _disposed = true;
        }
        #endregion
    }
}
