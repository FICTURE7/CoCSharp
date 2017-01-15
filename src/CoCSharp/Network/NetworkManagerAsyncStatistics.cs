using System.Threading;

namespace CoCSharp.Network
{
    /// <summary>
    /// Provides statistics for <see cref="NetworkManagerAsync"/>. This is mostly for debugging
    /// purposes.
    /// </summary>
    public class NetworkManagerAsyncStatistics
    {
        private static int s_nextTokenID;
        internal static int NextTokenID
        {
            get
            {
                s_nextTokenID++;
                return s_nextTokenID;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkManagerAsyncStatistics"/> class.
        /// </summary>
        public NetworkManagerAsyncStatistics()
        {
            // Space
        }

        /// <summary>
        /// Gets the total number of bytes transfered. That is the
        /// total number of bytes sent and received.
        /// </summary>
        public long TotalByteTransfered { get { return TotalByteSent + TotalByteReceived; } }

        internal long _totalSent;
        /// <summary>
        /// Gets the total number of bytes sent.
        /// </summary>
        public long TotalByteSent
        {
            get
            {
                return Thread.VolatileRead(ref _totalSent);
            }
        }

        internal long _totalReceived;
        /// <summary>
        /// Gets the total number of bytes received.
        /// </summary>
        public long TotalByteReceived
        {
            get
            {
                return Thread.VolatileRead(ref _totalReceived);
            }
        }

        internal long _totalMessagesSent;
        /// <summary>
        /// Gets the total number of messages sent.
        /// </summary>
        public long TotalMessagesSent
        {
            get
            {
                return Thread.VolatileRead(ref _totalMessagesSent);
            }
        }

        internal long _totalMessagesReceived;
        /// <summary>
        /// Gets the total number of messages received.
        /// </summary>
        public long TotalMessagesReceived
        {
            get
            {
                return Thread.VolatileRead(ref _totalMessagesReceived);
            }
        }
    }
}
