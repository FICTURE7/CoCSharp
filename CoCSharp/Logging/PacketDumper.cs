using CoCSharp.Networking;
using CoCSharp.Networking.Packets;
using System;
using System.IO;

namespace CoCSharp.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class PacketDumper
    {
        /// <summary>
        /// 
        /// </summary>
        public PacketDumper()
        {
            LoggingDirectory = "packet_dumps";
            if (!Directory.Exists(LoggingDirectory))
                Directory.CreateDirectory(LoggingDirectory);

            //TODO: Compress old logs into a zip file.
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggingDirectory"></param>
        public PacketDumper(string loggingDirectory)
        {
            LoggingDirectory = loggingDirectory;
            if (!Directory.Exists(loggingDirectory))
                Directory.CreateDirectory(loggingDirectory);

            //TODO: Compress old logs into a zip file.
            
        }

        public bool Active { get; set; }
        public string LoggingDirectory { get; set; }

        public void LogPacket(IPacket packet, PacketDirection direction, byte[] decryptedPacket)
        {
            //if (!Active) 
            //    return; // throw expection

            var packetType = packet.GetType();
            var fileTime = DateTime.Now.ToString("[~HH.mm.ss.fff] ");
            var filePrefix = direction == PacketDirection.Server ? "[CLIENT 2 SERVER] " :
                                                                   "[SERVER 2 CLIENT] ";
            var fileName = string.Format("{0} {1} {2} - 0x{3:X2} ({4})", fileTime,
                                                                         filePrefix,
                                                                         packetType.Name,
                                                                         packet.ID, // hex
                                                                         packet.ID); // decimal 
            var filePath = Path.Combine(LoggingDirectory, fileName);
            File.WriteAllBytes(filePath, decryptedPacket);
        }
    }
}
