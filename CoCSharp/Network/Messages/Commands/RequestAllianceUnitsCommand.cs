using System;

namespace CoCSharp.Network.Messages.Commands
{
    /// <summary>
    /// Request alliance units command
    /// </summary>
    public class RequestAllianceUnitsCommand : Command
    {
        /// <summary>
        /// RequestAllianceUnitsCommand()
        /// </summary>
        public RequestAllianceUnitsCommand()
        {

        }
        /// <summary>
        /// Get the ID of the <see cref="RequestAllianceUnitsCommand"/>
        /// </summary>
        public override int ID
        {
            get
            {
                return 540;
            }
        }
        /// <summary>
        /// FlagHasRequestMessage
        /// </summary>
        public byte FlagHasRequestMessage;
        /// <summary>
        /// Message
        /// </summary>
        public string Message;
        /// <summary>
        /// MessageLength
        /// </summary>
        public int MessageLength;
        /// <summary>
        /// Unknown
        /// </summary>
        public uint Unknown1;
        /// <summary>
        /// Reads the <see cref="RequestAllianceUnitsCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="RequestAllianceUnitsCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);
            FlagHasRequestMessage = reader.ReadByte();
            Message = reader.ReadString();
            MessageLength = reader.ReadInt32();
            Unknown1 = reader.ReadUInt32();
        }

        /// <summary>
        /// Writes the <see cref="RequestAllianceUnitsCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="RequestAllianceUnitsCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);
            writer.Write(FlagHasRequestMessage);
            writer.Write(Message);
            writer.Write(MessageLength);
            writer.Write(Unknown1);
        }
    }
}
