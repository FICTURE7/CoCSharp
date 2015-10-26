using System;

namespace CoCSharp.Networking
{
    public class NetworkManagerAsyncSettings
    {
        public NetworkManagerAsyncSettings()
        {
            // Space
        }

        private int m_BufferSize = 65535;
        public int BufferSize
        {
            get
            {
                return m_BufferSize;
            }
            set
            {
                if (value <= PacketExtractor.HeaderSize)
                    throw new Exception("BufferSize cannot be less or equal to header size (7).");
                m_BufferSize = value;
            }
        }

        private int m_ReceiveOpCount = 25;
        public int ReceiveOperationCount
        {
            get
            {
                return m_ReceiveOpCount;
            }
            set
            {
                if (value <= 0)
                    throw new Exception("ReceiveOperationCount cannot be less or equal to 0.");
                m_ReceiveOpCount = value;
            }
        }

        private int m_SendOpCount = 25;
        public int SendOperationCount
        {
            get
            {
                return m_SendOpCount;
            }
            set
            {
                if (value <= 0)
                    throw new Exception("SendOperationCount cannot be less or equal to 0.");
                m_SendOpCount = value;
            }
        }
    }
}
