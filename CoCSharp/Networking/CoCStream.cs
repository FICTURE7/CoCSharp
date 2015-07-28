using System;
using System.IO;
using System.Net.Sockets;

namespace CoCSharp.Networking
{
    public class CoCStream : Stream // will stop using this stuff.
    {
        public CoCStream(Socket connection)
        {
            //TODO: Do some renaming
            this.Connection = connection;
            this.ReadBuffer = new MemoryStream(4096);
            this.WriteBuffer = new MemoryStream(4096);
        }

        #region Properties
        public MemoryStream ReadBuffer { get; set; } // incoming packet buffer
        public MemoryStream WriteBuffer { get; set; } // outgoing packet buffer

        public override bool CanTimeout { get { return false; } }
        public override bool CanWrite { get { return true; } }
        public override bool CanRead { get { return true; } }
        public override int ReadTimeout { get; set; }
        public override int WriteTimeout { get; set; }
        public override long Length { get { throw new NotSupportedException(); } }
        public override bool CanSeek { get { throw new NotSupportedException(); } }
        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        private Socket Connection { get; set; }
        #endregion

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var bytesRead = Connection.Receive(buffer, offset, count, SocketFlags.None);
            ReadBuffer.Write(buffer, 0, bytesRead); // saves it to a buffer incase of fragmentation

            ReadBuffer.Seek(0, SeekOrigin.End); // continue stream
            ReadBuffer.Write(buffer, 0, bytesRead);
            ReadBuffer.Seek(0, SeekOrigin.Begin);

            return bytesRead;
        }

        public int ReadToBuffer() // receive data from socket and saves it to read buffer
        {
            var buffer = new byte[1024];
            var bytesRead = Connection.Receive(buffer, 0, 1024, SocketFlags.None);

            ReadBuffer.Seek(0, SeekOrigin.End); // continue stream
            ReadBuffer.Write(buffer, 0, bytesRead);
            ReadBuffer.Seek(0, SeekOrigin.Begin);

            return bytesRead;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Connection.Send(buffer, 0, count, SocketFlags.None);
        }
    }
}
