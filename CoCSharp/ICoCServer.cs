using System.Net;

namespace CoCSharp
{
    /// <summary>
    /// Defines what a basic CoCServer should implement.
    /// </summary>
    public interface ICoCServer
    {
        /// <summary>
        /// Start the server.
        /// </summary>
        void Start(EndPoint endPoint);

        /// <summary>
        /// Stops the server.
        /// </summary>
        void Stop();

        /// <summary>
        /// Logs data with the specified parameters and log category.
        /// </summary>
        /// <param name="parameters">The parameters to log the packet with.</param>
        void Log(params object[] parameters);
    }
}
