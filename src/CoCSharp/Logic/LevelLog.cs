using System;
using System.Diagnostics;
using System.IO;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Provides methods to log information about a <see cref="Logic.Level"/>.
    /// </summary>
    public class LevelLog
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LevelLog"/> class with the specified
        /// <see cref="Level"/>.
        /// </summary>
        /// <param name="level"><see cref="Level"/> which is associated with this <see cref="LevelLog"/>.</param>
        protected internal LevelLog(Level level)
        {
            if (level == null)
                throw new ArgumentNullException(nameof(level));

            _level = level;
        }
        #endregion

        #region Fields & Properties
        /// <summary>
        /// Gets the <see cref="Logic.Level"/> associated with the <see cref="LevelLog"/>.
        /// </summary>
        protected Level Level => _level;

        private readonly Level _level;
        #endregion

        #region Methods
        /// <summary>
        /// Logs the specified message to the <see cref="LevelLog"/>.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public void Log(string message)
        {
            var fileName = _level.Token + "-level.log";

            var file = File.Open(fileName, FileMode.Append);
            using (var writer = new StreamWriter(file))
                writer.WriteLine(message);

            Debug.WriteLine(message);
        }
        #endregion
    }
}
