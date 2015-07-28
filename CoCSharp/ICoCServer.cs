using CoCSharp.Logging;
using System.Collections.Generic;

namespace CoCSharp
{
    /// <summary>
    /// Defines what a basic CoCServer should implement.
    /// </summary>
    public interface ICoCServer
    {
        /// <summary>
        /// Logs data with the given parameters.
        /// </summary>
        /// <param name="parameter">The parameters to teh packet with.</param>
        void Log(params object[] parameter);
    }
}
