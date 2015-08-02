namespace CoCSharp.Logging
{
    public interface ILogger
    {
        /// <summary>
        /// If the logger is logging data.
        /// </summary>
        bool Active { get; set; }
        /// <summary>
        /// Name of the file(including path) the logger is writing to.
        /// </summary>
        string FileLog { get; set; }

        /// <summary>
        /// Logs data.
        /// </summary>
        /// <param name="logCategory"></param>
        /// <param name="parameters"></param>
        void Log(LogCategory logCategory, params object[] parameters);
    }
}
