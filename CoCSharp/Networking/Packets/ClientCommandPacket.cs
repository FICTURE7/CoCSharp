using CoCSharp.Networking.Commands;
using System;
using System.Collections.Generic;

namespace CoCSharp.Networking.Packets
{
    public class ClientCommandPacket : IPacket
    {
        public ushort ID { get { return 0x3716; } }

        public int Subtick;
        public int Checksum;
        public ICommand[] Commands;

        private static Dictionary<int, Type> CommandDictionary { get; set; }

        public void ReadPacket(PacketReader reader)
        {
            if (CommandDictionary == null)
                InitializeCommandDictionary();

            Subtick = reader.ReadInt32();
            Checksum = reader.ReadInt32();
            Commands = new ICommand[reader.ReadInt32()];

            for (int i = 0; i < Commands.Length; i++)
            {
                var commandID = reader.ReadInt32();
                Commands[i] = CreateCommandInstance(commandID);
                if (Commands[i] == null)
                    break;
                Commands[i].ReadCommand(reader);
            }
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteInt32(Subtick);
            writer.WriteInt32(Checksum);
            writer.WriteInt32(Commands.Length);
            for (int i = 0; i < Commands.Length; i++)
            {
                writer.WriteInt32(Commands[i].ID);
                Commands[i].WriteCommand(writer);
            }
        }

        private static void InitializeCommandDictionary()
        {
            CommandDictionary = new Dictionary<int, Type>();
            CommandDictionary.Add(new BuyBuildingCommand().ID, typeof(BuyBuildingCommand));
            CommandDictionary.Add(new CollectResourcesCommand().ID, typeof(CollectResourcesCommand));
        }

        private static ICommand CreateCommandInstance(int id)
        {
            var packetType = (Type)null;
            if (!CommandDictionary.TryGetValue(id, out packetType))
                return null;
            return (ICommand)Activator.CreateInstance(packetType); // creates an instance for that packetid
        }
    }
}
