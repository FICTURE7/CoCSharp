using System.Threading;

namespace CoCSharp.Network
{
    /// <summary>
    /// Provides statistics for <see cref="NetworkManagerAsync"/>. This is mostly for debugging
    /// purposes.
    /// </summary>
    public class NetworkManagerAsyncStatistics
    {
        private static int s_nextTokenId;
        internal static int NextTokenId => ++s_nextTokenId;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkManagerAsyncStatistics"/> class.
        /// </summary>
        public NetworkManagerAsyncStatistics()
        {
            // Space
        }
        #endregion

        #region Fields & Properties
        internal long _totalSent;
        internal long _totalReceived;
        internal long _totalMessagesSent;
        internal long _totalMessagesReceived;

        /// <summary>
        /// Gets the total number of bytes transfered. That is the
        /// total number of bytes sent and received.
        /// </summary>
        public long TotalByteTransfered => TotalByteSent + TotalByteReceived;

        /// <summary>
        /// Gets the total number of bytes sent.
        /// </summary>
        public long TotalByteSent => Thread.VolatileRead(ref _totalSent);

        /// <summary>
        /// Gets the total number of bytes received.
        /// </summary>
        public long TotalByteReceived => Thread.VolatileRead(ref _totalReceived);

        /// <summary>
        /// Gets the total number of messages sent.
        /// </summary>
        public long TotalMessagesSent => Thread.VolatileRead(ref _totalMessagesSent);

        /// <summary>
        /// Gets the total number of messages received.
        /// </summary>
        public long TotalMessagesReceived => Thread.VolatileRead(ref _totalMessagesReceived);
        #endregion
    }
}
